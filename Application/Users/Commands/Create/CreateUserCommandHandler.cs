using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Commands.Create
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
    {
        #region Private Fields

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly EncryptionHelper _encryptionHelper;
        private readonly IValidator<CreateUserCommand> _validator;

        #endregion

        #region Constructor

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IConfiguration configuration,
            IPasswordHasher passwordHasher,
            ILogger<CreateUserCommandHandler> logger, 
            IValidator<CreateUserCommand> validator)
            => (_userRepository, _configuration, _passwordHasher, _logger, validator) =
                (userRepository, configuration, passwordHasher, logger, validator);

        #endregion

        #region Handler Method

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validação antes de prosseguir com o comando
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Exceção ou retorno de erro para falha na validação
                throw new TickestException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            _logger.LogInformation("Iniciando a criação de usuário: {Email}", request.Email);
            await CheckIfEmailExists(request.Email);

            var user = CreateUser(request);
            await _userRepository.AddAsync(user);

            _logger.LogInformation("Usuário criado com sucesso: {Email}", request.Email);
            return MapResponse(user);
        }

        #endregion

        #region Private Methods

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
            // Utilizando o EncryptionHelper para gerar o salt e o hash da senha
            var salt = EncryptionHelper.CreateSaltaKey(32);  // Exemplo: Gerando um salt de 32 caracteres
            var passwordHash = EncryptionHelper.CreatePasswordHash(request.Senha, salt);  // Criando o hash da senha

            return new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash,
                Salt = salt,  // Armazenando o salt
                RegistrationDate = DateTime.UtcNow,
                IsActive = request.IsActive
            };
        }

        private CreateUserResponse MapResponse(User user) =>
            new CreateUserResponse(user.Id, user.Email, user.Name);

        #endregion
    }
}
