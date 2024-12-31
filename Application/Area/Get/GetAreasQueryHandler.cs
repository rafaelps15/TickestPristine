using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Area.Get;

internal sealed class GetAreasQueryHandler(IApplicationDbContext context, IAreaRepository areaRepository)
    : IQueryHandler<GetAreasQuery, List<AreaResponse>>
{
    public async Task<Result<List<AreaResponse>>> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        // Busca as áreas com especialidades
        var areas = await areaRepository.GetAreasWithSpecialtiesByIdsAsync(request.AreaIds,cancellationToken);

        if (areas == null || areas.Any())
        {
            throw new TickestException("Nenhuma área encontrada.");
        }

        // Mapeia as áreas para DTO(modelos de resposta)
        var areaResponses = areas.Select(area => new AreaResponse
        (
            area.Id,
            area.Name,
            area.Description,
            area.Specialty?.Name ?? "Nenhuma especialidade atribuída"
        )).ToList();

        return Result.Success(areaResponses);
    }
}
