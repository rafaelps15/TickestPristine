using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Login
{
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<string>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPasswordHasher _passwordHasher;

        // Injeção de dependências
        public LoginCommandHandler(
            IAuthService authService,
            IUserRepository userRepository,
            ILogger<LoginCommandHandler> logger,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher)
            => (_authService, _userRepository, _logger, _tokenProvider, _passwordHasher) = (authService, userRepository, logger, tokenProvider, passwordHasher);

        public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            // Validar se o usuário existe pelo email
            var user = await _userRepository.GetUserByEmailAsync(command.Email);

            if (user is null)
            {
                _logger.LogWarning($"Usuário não encontrado para o email: {command.Email}");
                return Result.Failure<string>(UserErrors.NotFoundByEmail);
            }

            // Obter o usuário atual
            var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

            if (currentUser == null)
            {
                _logger.LogError("Usuário não autenticado.");
                throw new TickestException("Usuário não autenticado.");
            }

            _logger.LogInformation($"Usuário {command.Email} tentando fazer login.");

            // Verificar se a senha informada é válida usando o PasswordHasher
            bool verified = _passwordHasher.Verify(command.Password, user.PasswordHash);

            if (!verified)
            {
                _logger.LogWarning($"Falha na tentativa de login para o email: {command.Email}. Senha incorreta.");
                return Result.Failure<string>("Senha incorreta.");
            }

            // Gerar o token de autenticação
            string token = _tokenProvider.Create(user);

            // Retornar o token em caso de sucesso
            _logger.LogInformation($"Login bem-sucedido para o usuário: {command.Email}");
            return Result.Success(token);
        }
    }
}
