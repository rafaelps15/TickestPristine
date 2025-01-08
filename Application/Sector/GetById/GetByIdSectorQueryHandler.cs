using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Sectors.Get;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Sectors.GetById;

internal sealed class GetByIdSectorQueryHandler(
    ISectorRepository sectorRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<GetByIdSectorQueryHandler> logger 
) : IQueryHandler<GetByIdSectorQuery, SectorResponse>
{
    public async Task<Result<SectorResponse>> Handle(GetByIdSectorQuery query, CancellationToken cancellationToken)
    {
        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "ViewSectors");

        logger.LogInformation("Usuário {UserId} autorizado a visualizar setor.", currentUser.Id);

        #endregion

        #region Obtenção do Setor

        var sector = await sectorRepository.GetByIdAsync(query.id);

        if (sector == null)
        {
            logger.LogWarning("Setor com o ID {SectorId} não encontrado.", query.id);
            throw new TickestException($"Setor com o ID {query.id} não encontrado.");
        }

        #endregion

        #region Mapeamento e Retorno

        var response = new SectorResponse(
            sector.Id,
            sector.Name,
            sector.Description ?? "Sem descrição",
            sector.SectorManager?.Name ?? "Nenhum usuário responsável",
            sector.Departments?.FirstOrDefault()?.Name ?? "Nenhum departamento atribuído"
        );

        #endregion

        logger.LogInformation("Setor {SectorId} encontrado e retornado com sucesso.", query.id);

        return Result.Success(response);
    }
}
