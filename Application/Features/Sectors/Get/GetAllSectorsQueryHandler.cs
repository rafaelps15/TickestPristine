using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Sectors.Get;

internal sealed class GetAllSectorsQueryHandler(
    ISectorRepository sectorRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<GetAllSectorsQueryHandler> logger 
) : IQueryHandler<GetAllSectorsQuery, List<SectorResponse>>
{
    public async Task<Result<List<SectorResponse>>> Handle(GetAllSectorsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a obtenção de todos os setores.");

        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "ViewSectors");

        logger.LogInformation("Usuário {UserId} autorizado a visualizar setores.", currentUser.Id);

        #endregion

        #region Obtenção dos Setores

        var sectors = await sectorRepository.GetAllAsync();

        if (sectors == null || !sectors.Any())
        {
            logger.LogWarning("Nenhum setor encontrado.");
            throw new TickestException("Nenhum setor encontrado.");
        }

        #endregion

        #region Mapeamento e Retorno

        var response = sectors.Select(sector => new SectorResponse(
            sector.Id,
            sector.Name,
            sector.Description ?? "Sem descrição",
            sector.SectorManager?.Name ?? "Nenhum usuário responsável",
            sector.Departments?.FirstOrDefault()?.Name ?? "Nenhum departamento atribuído"
        )).ToList();

        #endregion

        logger.LogInformation("Setores encontrados e retornados com sucesso.");

        return Result.Success(response);
    }
}
