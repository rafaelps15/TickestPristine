using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Common;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Infrastructure.Configurations;

namespace Tickest.Infrastructure.Authentication;

internal class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenProvider _tokenProvider;
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenProvider tokenProvider,
        IHttpContextAccessor httpContextAccessor,
        JwtConfiguration jwtConfiguration) =>

        (_userRepository, _refreshTokenRepository, _tokenProvider, _httpContextAccessor, _jwtConfiguration) =
            (userRepository, refreshTokenRepository, tokenProvider, httpContextAccessor, jwtConfiguration);

    /// <summary>
    /// Autentica o usuário e retorna um token JWT.
    /// </summary>
    public async Task<TokenResponse> AuthenticateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var expirationInMinutes = _jwtConfiguration.ExpirationInMinutes;
        var token = _tokenProvider.Create(user, expirationInMinutes);
        var refreshToken = CreateRefreshToken(user);

        // Salvar o refreshToken
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        // Gerar o token de resposta com a data de expiração
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationInMinutes);

        return new TokenResponse(token, refreshToken.Token, expiresAt);
    }

    /// <summary>
    /// Obtém o usuário atual a partir do contexto HTTP autenticado.
    /// </summary>
    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var principal = await GetPrincipalFromContextAsync();
        if (principal is null) return null;

        var userId = ExtractUserIdFromClaims(principal.Claims);
        if (userId == Guid.Empty) return null;

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        ValidadeUserIsActive(user);

        return user;
    }

    /// <summary>
    /// Renova o token JWT utilizando o refresh token fornecido.
    /// </summary>
    public async Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result<string>.Failure("O token de atualização é inválido.");

        var user = await _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken);
        if (user is null || !user.IsActive)
            return Result<string>.Failure("Token inválido ou usuário inativo.");

        // Usando a configuração para definir o tempo de expiração do novo token
        var newJwtToken = _tokenProvider.Create(user, _jwtConfiguration.ExpirationInMinutes);

        // Gerando um novo RefreshToken
        var newRefreshToken = GenerateNewRefreshToken();

        // Criando uma nova instância de RefreshToken e associando ao usuário
        var refreshTokenEntity = new RefreshToken(user.Id, newRefreshToken, DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationInMinutes));

        // Adicionando o novo RefreshToken na coleção de RefreshTokens do usuário
        user.RefreshTokens.Add(refreshTokenEntity);

        // Salvando a atualização do usuário
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result<string>.Success(newJwtToken);
    }


    /// <summary>
    /// Obtém o principal do contexto HTTP autenticado.
    /// </summary>
    private async Task<ClaimsPrincipal?> GetPrincipalFromContextAsync()
    {
        var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync("Bearer");
        return authenticationResult.Succeeded ? authenticationResult.Principal : null;
    }

    /// <summary>
    /// Extrai o ID do usuário dos claims.
    /// </summary>
    private static Guid ExtractUserIdFromClaims(IEnumerable<Claim> claims)
    {
        var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(userIdClaim?.Value, out var userId) ? userId : Guid.Empty;
    }

    /// <summary>
    /// Valida se o usuário está ativo.
    /// </summary>
    private static void ValidadeUserIsActive(User? user)
    {
        if (user is null || !user.IsActive)
            throw new InvalidOperationException("Usuário inativo ou não encontrado.");
    }

    /// <summary>
    /// Gera um novo refresh token.
    /// </summary>
    private static string GenerateNewRefreshToken() =>
        Guid.NewGuid().ToString("N");

    private static RefreshToken CreateRefreshToken(User user)
    {
        var expirationInMinutes = DateTime.UtcNow.AddDays(30);
        var token = Guid.NewGuid().ToString("N");
        return new RefreshToken(user.Id, token, expirationInMinutes);
    }

    public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken);
    }

}
