using FluentValidation;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    #region Private Fields

    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IValidator<CreateUserCommand> _validator;

    #endregion

    #region Constructor

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        ILogger<CreateUserCommandHandler> logger,
        IValidator<CreateUserCommand> validator)
        => (_userRepository, _roleRepository, _passwordHasher, _logger, _validator) =
            (userRepository, roleRepository, passwordHasher, logger, validator);

    #endregion

    #region Handler Method

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new TickestException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            _logger.LogInformation("Iniciando a criação de usuário: {Email}", request.Email);
            await CheckIfEmailExists(request.Email);

            var user = CreateUser(request);

            // Atribuindo Roles e especialidades
            await AssignRolesAndSpecializations(user, request);

            await _userRepository.AddAsync(user);

            _logger.LogInformation("Usuário criado com sucesso: {Email}", request.Email);
            return MapResponse(user);
        }
        catch (Exception ex)
        {
            // Log and handle the exception using the middleware's structure
            _logger.LogError(ex, "Erro ao criar usuário: {Email}", request.Email);
            throw new TickestException("Ocorreu um erro ao criar o usuário. Tente novamente mais tarde.");
        }
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
        var passwordHash = EncryptionHelper.CreatePasswordHash(request.Password, salt);  // Criando o hash da senha

        return new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHash,
            Salt = salt,  // Armazenando o salt
            FirstRegistrationDate  = DateTime.UtcNow,
            IsActive = request.IsActive
        };
    }
    private async Task AssignRolesAndSpecializations(User user, CreateUserCommand request)
    {
        // Atribuindo as roles
        // Obtemos as roles do repositório passando os nomes das roles que foram enviadas no comando
        var roles = await _roleRepository.GetRolesByNamesAsync(request.RoleNames);

        // Atribuímos as roles à propriedade 'Roles' do usuário
        user.Roles = roles.ToList(); // Converte para lista, caso seja necessário

        // Atribuindo especialidades (Setor, Departamento, Área, Especialidade)
        user.Sector = request.Sector;           // Setor
        user.Department = request.Department;   // Departamento
        user.Area = request.Area;               // Área
        user.Specialization = request.Specialization; // Especialidade
    }

    private CreateUserResponse MapResponse(User user) =>
        new CreateUserResponse(user.Id, user.Email, user.Name);

    #endregion
}

