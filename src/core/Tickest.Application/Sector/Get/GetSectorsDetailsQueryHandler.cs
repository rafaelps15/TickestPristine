using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Sector.Get;

internal sealed class GetSectorsDetailsQueryHandler(ISectorRepository sectorRepository)
    : IQueryHandler<GetSectorsDetailsQuery, List<SectorResponse>>
{
    public async Task<Result<List<SectorResponse>>> Handle(GetSectorsDetailsQuery query, CancellationToken cancellationToken)
    {
        if (query.SectorId == Guid.Empty)
        {
            throw new TickestException("ID do setor invalido.");
        }

        var sectors = await sectorRepository.GetAllAsync(cancellationToken: cancellationToken);

        if (!sectors.Any())
        {
            throw new TickestException($"Setor com ID {query.SectorId} nao encontrado.");
        }

        var response = sectors.Select(sector => new SectorResponse
        {
            Id = sector.Id,
            Name = sector.Name,
            DepartmentName = sector.Department?.Name ?? "Nenhum departamento atribuido",
            ResponsibleUserName = sector.ResponsibleUser?.Name ?? "Nenhum usuario responsavel",
            AreaNames = sector.Areas?.Select(area => area.Name).ToList() ?? new List<string>()
        }).ToList();

        return Result.Success(response);
    }
}
