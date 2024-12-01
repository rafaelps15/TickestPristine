using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Common;
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
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenProvider tokenProvider,
        IHttpContextAccessor httpContextAccessor,
        JwtConfiguration jwtConfiguration,
        ILogger<AuthService> logger) =>
        (_userRepository, _refreshTokenRepository, _tokenProvider, _httpContextAccessor, _jwtConfiguration, _logger) =
            (userRepository, refreshTokenRepository, tokenProvider, httpContextAccessor, jwtConfiguration, logger);

    public async Task<TokenResponse> AuthenticateAsync(User user, CancellationToken cancellationToken)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        try
        {
            var expirationInMinutes = _jwtConfiguration.ExpirationInMinutes;
            var token = _tokenProvider.Create(user, expirationInMinutes);
            var refreshToken = CreateRefreshToken(user);

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            var expiresAt = DateTime.UtcNow.AddMinutes(expirationInMinutes);

            return new TokenResponse(token, refreshToken.Token, expiresAt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao autenticar usuário e gerar tokens.");
            throw new AuthenticationException("Erro ao autenticar o usuário.", ex);
        }
    }

    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var principal = await GetPrincipalFromContextAsync();
        if (principal == null) return null;

        var userId = ExtractUserIdFromClaims(principal.Claims);
        if (userId == Guid.Empty) return null;

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        EnsureUserIsActive(user);

        return user;
    }

    public async Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Result<string>.Failure("O token de atualização é inválido.");
        }

        var user = await _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken);
        if (user == null || !user.IsActive)
        {
            return Result<string>.Failure("Token inválido ou usuário inativo.");
        }

        var newJwtToken = _tokenProvider.Create(user, _jwtConfiguration.ExpirationInMinutes);
        var newRefreshToken = GenerateNewRefreshToken();

        var refreshTokenEntity = new RefreshToken(user.Id, newRefreshToken, DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationInMinutes));
        user.RefreshTokens.Add(refreshTokenEntity);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result<string>.Success(newJwtToken);
    }

    private async Task<ClaimsPrincipal?> GetPrincipalFromContextAsync()
    {
        var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync("Bearer");
        return authenticationResult.Succeeded ? authenticationResult.Principal : null;
    }

    private static Guid ExtractUserIdFromClaims(IEnumerable<Claim> claims)
    {
        var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(userIdClaim?.Value, out var userId) ? userId : Guid.Empty;
    }

    private static void EnsureUserIsActive(User? user)
    {
        if (user == null || !user.IsActive)
        {
            throw new InvalidOperationException("Usuário inativo ou não encontrado.");
        }
    }

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
