using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Areas.Get;

internal sealed class GetAreasQueryHandler(IApplicationDbContext context, IAreaRepository areaRepository)
    : IQueryHandler<GetAreasQuery, List<AreaResponse>>
{
    public async Task<Result<List<AreaResponse>>> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        var areas = await areaRepository.GetAllAsync(cancellationToken);

        if (areas == null || areas.Any())
        {
            throw new TickestException("Nenhuma área encontrada.");
        }

        // Mapeia as áreas para DTO (modelos de resposta)
        var areaResponses = areas.Select(area => new AreaResponse
        (
            area.Id,
            area.Name,
            area.Description,
            area.Users.SelectMany(user => user.Specialties) // Seleciona a primeira especialidade do usuário ou um valor default
                      .FirstOrDefault()?.Name ?? "Nenhuma especialidade atribuída"
        )).ToList();


        return Result.Success(areaResponses);
    }
}
