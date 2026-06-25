using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ISectorRepository sectorRepository,
    ISpecialtyRepository specialtyRepository,
    IUnitOfWork unitOfWork,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        var existingUser = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (existingUser is not null)
        {
            logger.LogError("O e-mail {Email} já está em uso.", command.Email);
            throw new TickestException("O e-mail fornecido já está em uso.");
        }

        var existingEmployeeCode = await userRepository.GetByEmployeeCodeAsync(command.EmployeeCode, cancellationToken);

        if (existingEmployeeCode is not null)
        {
            logger.LogError("O código de funcionário {EmployeeCode} já está em uso.", command.EmployeeCode);
            throw new TickestException("O código de funcionário fornecido já está em uso.");
        }

        var role = await roleRepository.GetByNameAsync(SystemRoles.Collaborator, cancellationToken);

        if (role is null)
        {
            throw new TickestException("Função padrão de colaborador não encontrada ou inativa.");
        }

        // Temporário para teste: valida setor apenas quando informado.
        if (command.SectorId.HasValue)
        {
            var sector = await sectorRepository.GetByIdAsync(command.SectorId.Value, true, cancellationToken);

            if (sector is null)
            {
                throw new TickestException("Setor inválido.");
            }
        }

        // Temporário para teste: aceita cadastro sem especialidades.
        var specialtyIds = command.SpecialtyIds?.Distinct().ToList() ?? [];

        foreach (var specialtyId in specialtyIds)
        {
            var specialty = await specialtyRepository.GetByIdAsync(specialtyId, true, cancellationToken);

            if (specialty is null)
            {
                throw new TickestException("Especialidade inválida.");
            }
        }

        var user = new User
        {
            Name = command.Name,
            EmployeeCode = command.EmployeeCode,
            Email = command.Email,
            PasswordHash = passwordHasher.Hash(command.Password),
            RoleId = role.Id,
            SectorId = command.SectorId
        };

        foreach (var specialtyId in specialtyIds)
        {
            user.UserSpecialties.Add(new UserSpecialty
            {
                UserId = user.Id,
                SpecialtyId = specialtyId
            });
        }

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Novo usuário criado com ID {UserId}.", user.Id);

        return Result.Success(user.Id);
    }
}
