using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Area.Get;

internal sealed class GetAreasQueryHandler(IAreaRepository areaRepository)
    : IQueryHandler<GetAreasQuery, List<AreaResponse>>
{
    public async Task<Result<List<AreaResponse>>> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        var areas = await areaRepository.GetAllAsync(cancellationToken: cancellationToken);

        if (!areas.Any())
        {
            throw new TickestException("Nenhuma area encontrada.");
        }

        var response = areas.Select(area => new AreaResponse
        {
            Id = area.Id,
            Name = area.Name,
            Description = area.Description,
            SpecialtyName = area.Specialty?.Name ?? "Nenhuma especialidade atribuida"
        }).ToList();

        return Result.Success(response);
    }
}
