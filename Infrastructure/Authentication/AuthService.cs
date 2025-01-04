using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.DTOs;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    #region Dependencies

    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        IUserRepository userRepository,
        ITokenProvider tokenProvider,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger,
        IRefreshTokenRepository refreshTokenRepository) =>
        (_userRepository, _tokenProvider, _httpContextAccessor, _logger, _refreshTokenRepository) =
        (userRepository, tokenProvider, httpContextAccessor, logger, refreshTokenRepository);

    #endregion

    #region Public Methods

    public async Task<TokenResponse> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken)
                    ?? throw new TickestException("Usuário não encontrado");

        ValidateUserState(user);
        ValidatePassword(password, user);

        var accessToken = _tokenProvider.Create(user);
        _logger.LogInformation("Usuário {Email} autenticado com sucesso.", email);

        return new TokenResponse { AccessToken = accessToken };
    }

    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var email = GetEmailFromToken();
        return await _userRepository.GetUserByEmailAsync(email, cancellationToken)
               ?? throw new TickestException("Usuário não encontrado.");
    }

    public async Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await GetUserByRefreshTokenAsync(refreshToken, cancellationToken);
        var newToken = _tokenProvider.Create(user);

        return Result.Success(newToken);
    }

    #endregion

    #region Private Helpers

    private void ValidateUserState(User user)
    {
        if (!user.IsActive)
            throw new TickestException("Usuário inativo.");
        if (user.IsDeleted)
            throw new TickestException("Usuário deletado.");
    }

    private void ValidatePassword(string password, User user)
    {
        if (!EncryptionHelper.CheckPasswod(password, user.Salt, user.PasswordHash))
            throw new TickestException("Credenciais inválidas");
    }

    private string GetEmailFromToken()
    {
        var email = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
        if (string.IsNullOrWhiteSpace(email))
            throw new TickestException("Usuário não autenticado.");
        return email;
    }

    private async Task<User> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
        if (refreshTokenEntity == null || refreshTokenEntity.ExpiresAt < DateTime.UtcNow)
            throw new TickestException("Refresh token inválido ou expirado.");

        return await _userRepository.GetByIdAsync(refreshTokenEntity.UserId)
               ?? throw new TickestException("Usuário associado ao refresh token não encontrado.");
    }

    #endregion
}
