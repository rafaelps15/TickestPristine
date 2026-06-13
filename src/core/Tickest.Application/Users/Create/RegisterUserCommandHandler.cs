using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IPermissionProvider permissionProvider,
    IAuthService authService,
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

        var role = await roleRepository.GetByIdAsync(command.RoleId, true, cancellationToken);

        if (role is null || permissionProvider.GetPermissionsForRole(role.Name).Count == 0)
        {
            logger.LogError("Função {RoleId} inválida para o novo usuário.", command.RoleId);
            throw new TickestException("Função inválida.");
        }

        var hasUsers = await userRepository.AnyAsync(cancellationToken);

        if (!hasUsers)
        {
            if (role.Name != SystemRoles.AdminMaster)
            {
                throw new TickestException("O primeiro usuário do sistema deve ser AdminMaster.");
            }
        }
        else
        {
            var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
            var rolePermissions = permissionProvider.GetPermissionsForRole(currentUser.Role.Name);

            if (!rolePermissions.Contains(SystemPermissions.ManageUsers))
            {
                logger.LogError(
                    "Usuário {UserId} com função {Role} não tem permissão para criar um novo usuário.",
                    currentUser.Id,
                    currentUser.Role.Name);

                throw new TickestException("Você não tem permissão para criar um novo usuário.");
            }
        }

        if (command.SectorId.HasValue)
        {
            var sector = await sectorRepository.GetByIdAsync(command.SectorId.Value, true, cancellationToken);

            if (sector is null)
            {
                throw new TickestException("Setor inválido.");
            }
        }

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
            Email = command.Email,
            EmployeeCode = command.EmployeeCode,
            Name = command.Name,
            PasswordHash = passwordHasher.Hash(command.Password),
            RoleId = command.RoleId,
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
