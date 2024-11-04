using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tickest.Domain.Contracts.Responses.UserResponses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Authentication.Commands.Register
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUserRepository usuarioRepository,
            IConfiguration configuration,
            IPasswordHasher hasherDeSenha,
            ILogger<CreateUserCommandHandler> logger)
            => (_userRepository, _configuration, _passwordHasher, _logger) = (usuarioRepository, configuration, hasherDeSenha, logger);

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            request.Validate();

            _logger.LogInformation("Iniciando a criação de usuário: {Email}", request.Email);
            await CheckIfEmailExists(request.Email);

            var user = CreateUser(request);
            await _userRepository.AddAsync(user, cancellationToken);

            _logger.LogInformation("Usuário criado com sucesso: {Email}", request.Email);
            return MapResponse(user);
        }

        private async Task CheckIfEmailExists(string email)
        {
            if (await _userRepository.DoesEmailExistAsync(email))
            {
                _logger.LogWarning("Tentativa de cadastro com email já existente: {Email}", email);
                throw new TickestException("Email já cadastrado.");
            }
        }

        private User CreateUser(CreateUserCommand request)
        {
            var (encryptedPassword, passwordSalt) = CreatePasswordHash(request.Senha);

            return new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = encryptedPassword,
                Salt = passwordSalt,
                DateRegistered = DateTime.UtcNow,
                IsActive = request.IsActive
            };
        }

        private (string EncryptedPassword, string PasswordSalt) CreatePasswordHash(string password)
        {
            var passwordSalt = _passwordHasher.GenerateSalt();
            var encryptedPassword = _passwordHasher.HashPassword(password, passwordSalt);
            return (encryptedPassword, passwordSalt);
        }

        private CreateUserResponse MapResponse(User user) =>
            new CreateUserResponse(user.Id, user.Email, user.Name);
    }
}