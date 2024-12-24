using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.DTOs;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    #region Fields

    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    private const int PasswordHashVersion = 2;

    #endregion

    #region Constructor

    public AuthService(
        IUserRepository userRepository,
        ITokenProvider tokenProvider,
        IPasswordHasher passwordHasher,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork) =>
        (_userRepository, _tokenProvider, _passwordHasher, _httpContextAccessor, _logger, _refreshTokenRepository, _unitOfWork) =
        (userRepository, tokenProvider, passwordHasher, httpContextAccessor, logger, refreshTokenRepository, unitOfWork);

    #endregion

    #region Public Methods

    public async Task<TokenResponse> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await ValidateUserCredentialsAsync(email, password, cancellationToken);

        var newPasswordHash = RehashIfNeeded(password, user.PasswordHash);
        if (newPasswordHash != null)
        {
            user.PasswordHash = newPasswordHash;
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // Cria o token JWT
        var token = _tokenProvider.Create(user);

        _logger.LogInformation($"Usuário {email} autenticado com sucesso.");

        return new TokenResponse
        {
            Token = token
        };
    }


    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var email = GetEmailFromContext();
        var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);

        if (user == null)
            throw new TickestException("Usuário não encontrado.");

        return user;
    }

    public async Task<Result<string>> RenewTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await GetUserByRefreshTokenAsync(refreshToken, cancellationToken);

        var token = _tokenProvider.Create(user);

        return Result.Success(token);
    }

    #endregion

    #region Private Methods

    public async Task<User> ValidateUserCredentialsAsync(string email, string password, CancellationToken cancellationToken) // Tornado público
    {
        var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
        if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
            throw new TickestException("Credenciais inválidas.");

        return user;
    }

    public async Task RehashPasswordAsync(User user, string password, CancellationToken cancellationToken) // Tornado público
    {
        var updatedHash = _passwordHasher.Hash(password);
        user.PasswordHash = updatedHash;
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    private string GetEmailFromContext()
    {
        var email = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
        if (string.IsNullOrEmpty(email))
            throw new TickestException("Usuário não autenticado.");

        return email;
    }

    private async Task<User> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _unitOfWork.RefreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
        if (refreshTokenEntity == null || refreshTokenEntity.ExpiresAt < DateTime.UtcNow)
            throw new TickestException("Refresh token inválido ou expirado.");

        var user = await _userRepository.GetByIdAsync(refreshTokenEntity.UserId);
        if (user == null)
            throw new TickestException("Usuário associado ao refresh token não encontrado.");

        return user;
    }

    #endregion

    #region Rehashing

    public string? RehashIfNeeded(string password, string passwordHash)
    {
        var (version, storedHash, salt) = ParseHash(passwordHash);

        if (version < PasswordHashVersion)
        {
            return _passwordHasher.Hash(password);
        }

        return null;
    }

    private static (int version, byte[] storedHash, byte[] salt) ParseHash(string passwordHash)
    {
        var parts = passwordHash.Split('-');
        if (parts.Length != 3)
            throw new TickestException("Formato do hash inválido.");

        var version = int.Parse(parts[0]);
        var storedHash = Convert.FromHexString(parts[1]);
        var salt = Convert.FromHexString(parts[2]);

        return (version, storedHash, salt);
    }

    #endregion
}
