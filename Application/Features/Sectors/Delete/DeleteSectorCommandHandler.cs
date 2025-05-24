using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Sectors.Delete;

internal sealed class DeleteSectorCommandHandler(
    ISectorRepository sectorRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<DeleteSectorCommandHandler> logger)
    : ICommandHandler<DeleteSectorCommand, Guid>
{

    public async Task<Result<Guid>> Handle(DeleteSectorCommand command, CancellationToken cancellationToken)
    {
        #region Validação de Permissões

        //var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        //await permissionProvider.ValidatePermissionAsync(currentUser, "CreateSector");

        #endregion

        #region Verificação do Setor

        var sector = await sectorRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sector is null)
        {
            logger.LogInformation("Setor com ID {SectorId} não encontrado.", command.Id);
            throw new TickestException("Setor não encontrado.");
        }

        #endregion

        #region Verificação de SoftDelete (Já foi desativado?)

        if (sector.IsDeleted)
        {
            logger.LogInformation("Setor com ID {SectorId} já foi desativado.", command.Id);
            throw new TickestException("Setor já foi desativado.");
        }

        #endregion

        #region Exclusão Suave (Soft Delete)

        sector.SoftDelete(); 

        #endregion

        #region Exclusão do Setor (Update no Banco de Dados)

        await sectorRepository.UpdateAsync(sector);
        logger.LogInformation("Setor com ID {SectorId} excluído com sucesso.", command.Id);

        #endregion

        #region Retorno de Resultado

        return Result.Success(command.Id);

        #endregion
    }
}
