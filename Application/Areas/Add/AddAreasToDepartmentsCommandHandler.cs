using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Areas.Add;

internal sealed class AddAreasToDepartmentsCommandHandler(
    IAreaRepository areaRepository,
    IDepartmentRepository departmentRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<AddAreasToDepartmentsCommandHandler> logger)
    : ICommandHandler<AddAreasToDepartmentsCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddAreasToDepartmentsCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Associando áreas ao departamento {DepartamentoId}", command.DepartmentId);

        #region Verificação de Permissão do Usuário

        //var currentUser = await authService.GetCurrentUserAsync(cancellationToken);

        //const string requiredPermission = "CanAddAreasToDepartments";
        //await permissionProvider.ValidatePermissionAsync(currentUser, requiredPermission);
        //logger.LogInformation("Usuário {UserId} autorizado para associar departamentos.", currentUser.Id);

        #endregion

        #region Validação dos Departamentos

        var department = await departmentRepository.GetByIdAsync(command.DepartmentId, cancellationToken);
        if (department is null)
        {
            logger.LogError("Departamento com ID {DepartmentId} não encontrado.", command.DepartmentId);
            throw new TickestException("Departamento não encontrado.");
        }

        #endregion

        #region Validação e Associação das Áreas 

        var areas = await areaRepository.GetAreasByIdsAsync(command.AreaIds, cancellationToken);
        if (!areas.Any())
        {
            logger.LogWarning("Nenhuma área encontrada para os IDs fornecidos.");
            throw new TickestException("Nenhuma área encontrada.");
        }

        var areasToAdd = await areaRepository.GetAvailableAsync(department, command.AreaIds, cancellationToken);

        if (!areasToAdd.Any())
        {
            logger.LogInformation("Nenhuma nova área a ser associada ao departamento {DepartmentId}.", command.DepartmentId);
            throw new TickestException("Todas as áreas fornecidas já estão associadas ao departamento.");
        }

        // Associando as áreas ao departamento -> AddRange adiciona todas as áreas de uma vez.
        department.Areas.AddRange(areasToAdd);

        await departmentRepository.UpdateAsync(department, cancellationToken);

        logger.LogInformation("As áreas foram associadas ao departamento {DepartmentId} com sucesso.", command.DepartmentId);

        #endregion

        return Result.Success(command.DepartmentId);
    }
}
