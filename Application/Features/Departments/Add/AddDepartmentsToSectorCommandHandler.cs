using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Departments.Add;

internal sealed class AddDepartmentsToSectorCommandHandler(
    ISectorRepository sectorRepository,
    IDepartmentRepository departmentRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<AddDepartmentsToSectorCommandHandler> logger)
    : ICommandHandler<AddDepartmentsToSectorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddDepartmentsToSectorCommand command, CancellationToken cancellationToken)
    {
        #region Log de Início

        logger.LogInformation("Associando departamentos ao setor {SectorId}.", command.SectorId);

        #endregion

        #region Verificação de Permissão do Usuário

        //var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        //if (currentUser == null)
        //{
        //    logger.LogError("Usuário não autenticado.");
        //    throw new TickestException("Usuário não autenticado.");
        //}

        //const string requiredPermission = "AdminMaster, AdminGeral, SectorManager";
        //await permissionProvider.ValidatePermissionAsync(currentUser, requiredPermission);
        //logger.LogInformation("Usuário {UserId} autorizado para associar departamentos.", currentUser.Id);

        #endregion

        #region Validação do Setor

        var sector = await sectorRepository.GetByIdAsync(command.SectorId, cancellationToken);
        if (sector == null)
        {
            logger.LogError("Setor com ID {SectorId} não encontrado.", command.SectorId);
            throw new TickestException("Setor não encontrado.");
        }

        #endregion

        #region Validação e Associação dos Departamentos

        var department = await departmentRepository.GetByIdAsync(command.DepartamentId, cancellationToken);
        if (department == null)
        {
            logger.LogError("Departamento com ID {DepartmentId} não encontrado.", command.DepartamentId);
            throw new TickestException("Departamento não encontrado.");
        }

        sector.Departments.Add(department);


        #endregion

        #region Persistência dos Departamentos

        await sectorRepository.UpdateAsync(sector,cancellationToken);
        logger.LogInformation("Departamentos associados ao setor {SectorId} com sucesso.", command.SectorId);

        #endregion

        #region Retorno

        return Result.Success(command.SectorId);

        #endregion
    }
}
