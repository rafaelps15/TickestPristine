using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Sectors.Create;

internal sealed class CreateDepartmentCommandHandler(
    IPermissionProvider permissionProvider,
    IAuthService authService,
    IDepartmentRepository departmentRepository,
    IUserRepository userRepository,
    ILogger<CreateDepartmentCommandHandler> logger)
    : ICommandHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo departamento.");

        #region Verificação de Permissão do Usuário

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "CreateDepartment");

        logger.LogInformation("Usuário {UserId} autorizado a criar um Departamento.", currentUser.Id);

        #endregion

        #region Validação do Usuário Responsável (Gestor do Departamento)

        if (command.DepartmentManagerId.HasValue)
        {
            // Validação para garantir que o usuário fornecido exista
            var departmentManager = await userRepository.GetByIdAsync(command.DepartmentManagerId.Value, cancellationToken);
            if (departmentManager == null)
            {
                logger.LogError("Gestor do departamento com ID {UserId} não encontrado.", command.DepartmentManagerId);
                throw new TickestException("Gestor do departamento não encontrado.");
            }
        }

        #endregion

        #region Criação do Novo Departamento

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        if (command.DepartmentManagerId.HasValue)
        {
            department.DepartmentManagerId = command.DepartmentManagerId.Value;
        }

        logger.LogInformation("Departamento preparado para persistência: {DepartmentId}.", department.Id);

        #endregion

        #region Persistência no Banco de Dados

        await departmentRepository.AddAsync(department, cancellationToken);

        logger.LogInformation($"Departamento {department.Name} criado com sucesso.");

        #endregion

        return Result.Success(department.Id);
    }
}
