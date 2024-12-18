using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Sector.Get;

////Caso necessario utilizar o banco de dados utilizar IApplicationDbContext context
internal sealed class GetSectorsDetailsQueryHandler(IApplicationDbContext context, ISectorRepository sectorRepository)
    : IQueryHandler<GetSectorsDetailsQuery, List<SectorResponse>>
{
    public async Task<Result<List<SectorResponse>>> Handle(GetSectorsDetailsQuery query, CancellationToken cancellationToken)
    {
        // Verifica se o ID do setor é válido
        if (query.SectorId == Guid.Empty)
        {
            throw new TickestException("ID do setor inválido.");
        }

        // Recupera o setor via repositório
        var sectors = await sectorRepository.GetAllAsync();

        if (sectors == null)
        {
            throw new TickestException($"Sector with ID {query.SectorId} not found.");
        }

        // Mapeia o setor para DTO
        var response = sectors.Select(sector => new SectorResponse
        {
            Id = sector.Id,
            Name = sector.Name,
            DepartmentName = sector.Department?.Name ?? "Nenhum departamento atribuído",
            ResponsibleUserName = sector.ResponsibleUser?.Name ?? "Nenhum usuário responsável",
            AreaNames = sector.Areas?.Select(area => area.Name).ToList() ?? new List<string>()
        }).ToList();

        return Result.Success(response);
    }
}
