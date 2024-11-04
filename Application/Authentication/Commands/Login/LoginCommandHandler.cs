using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Contracts.Responses.UserResponses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Helpers;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _usuarioRepository;
    private readonly JwtConfiguracao _jwtConfiguracao;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IAuthenticationService authenticationService,
        IUserRepository usuarioRepository,
        IOptions<JwtConfiguracao> jwtConfiguracao,
        ILogger<LoginCommandHandler> logger)
        => (_authenticationService, _usuarioRepository, _jwtConfiguracao, _logger) = (authenticationService, usuarioRepository, jwtConfiguracao.Value,logger);

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        request.validate();
        _logger.LogInformation($"Usuário {request.Email} tentando fazer login.");

        var user = await _usuarioRepository.ObterUsuarioPorEmailAsync(request.Email)
            ?? throw new TickestException("Usuário não encontrado.");

        ValidatePassword(request.Password, user.Salt, user.Password);
      
        var token = await _authenticationService.AuthenticateAsync(user);
        return new LoginResponse(user.Id, user.Email, user.Name, token);
    }

    private void ValidatePassword(string senha, string salt, string storedPassword)
    {
        var hasher = new PasswordHasher();
        var hashedPassword = hasher.HashPassword(senha, salt);

        if (!hashedPassword.Equals(storedPassword))
            throw new TickestException("Senha incorreta.");
    }
}
