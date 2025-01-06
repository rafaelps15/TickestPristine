using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Departments.Get;

////Caso necessario utilizar o banco de dados utilizar IApplicationDbContext context
internal sealed class GetSectorsDetailsQueryHandler(IApplicationDbContext context, ISectorRepository sectorRepository)
    : IQueryHandler<GetSectorsDetailsQuery, List<SectorResponse>>
{
    public async Task<Result<List<SectorResponse>>> Handle(GetSectorsDetailsQuery query, CancellationToken cancellationToken)
    {
        // Recupera o setor via repositório
        var sectors = await sectorRepository.GetAllAsync();

        if (sectors == null)
        {
            throw new TickestException($"Setor com o ID {query.SectorIds} não encontrado.");
        }

        // Mapeia o setor para DTO
        var response = sectors.Select(sector => new SectorResponse(
            sector.Id,
            sector.Name,
            sector.Description ?? "Sem descrição",
            sector.SectorManager?.Name ?? "Nenhum usuário responsável",  
            sector.Departments?.FirstOrDefault()?.Name ?? "Nenhum departamento atribuído"
        )).ToList();

        return Result.Success(response);
    }
}
