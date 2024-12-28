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
        // Recupera todas as áreas via repositório
        var areas = await areaRepository.GetAllAsync();

        if (areas == null || areas.Any())
        {
            throw new TickestException("Nenhuma área encontrada.");
        }

        // Mapeia as áreas para DTO
        var response = areas.Select(area => new AreaResponse
        {
            Id = area.Id,
            Name = area.Name,
            Description = area.Description,
            SpecialtyName = area.Specialty?.Name ?? "Nenhuma especialidade atribuída"
        }).ToList();


        return Result.Success(response);
    }
}
