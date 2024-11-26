using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IAuthService _authenticator;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IAuthService authenticator,
        IUserRepository userRepository,
        ILogger<LoginCommandHandler> logger)
        => (_authenticator, _userRepository, _logger) = (authenticator, userRepository, logger);

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Usuário {request.Email} tentando fazer login.");

        var user = await _userRepository.GetUserByEmailAsync(request.Email)
            ?? throw new TickestException("Usuário não encontrado.");

        ValidatePassword(request.Password, user.Salt, user.Password);

        var token = await _authenticator.AuthenticateAsync(user);
        return new LoginResponse(user.Id, user.Email, user.Name, token);
    }

    private static void ValidatePassword(string password, string salt, string storedPassword)
    {
        var hashedPassword = EncryptionHelper.CreatePasswordHash(password, salt);

        if (!hashedPassword.Equals(storedPassword))
            throw new TickestException("Senha incorreta.");
    }
}
