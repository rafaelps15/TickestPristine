using Microsoft.EntityFrameworkCore;
using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Sectors.Get;

internal sealed class GetSectorsDetailsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetSectorsDetailsQuery, List<SectorResponse>>
{
    public async Task<Result<List<SectorResponse>>> Handle(GetSectorsDetailsQuery query, CancellationToken cancellationToken)
    {
        if (query.SectorId == Guid.Empty)
        {
            throw new TickestException("ID do setor inválido.");
        }

        var sectors = await context.Sectors
            .AsNoTracking()
            .Include(s => s.Department)
            .Include(s => s.ResponsibleUser)
            .Where(s => s.Id == query.SectorId && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);

        if (sectors.Count == 0)
        {
            throw new TickestException($"Setor com ID {query.SectorId} não encontrado.");
        }

        var response = sectors.Select(s => new SectorResponse
        {
            Id = s.Id,
            Name = s.Name,
            DepartmentName = s.Department?.Name,
            ResponsibleUserName = s.ResponsibleUser?.Name
        }).ToList();

        return Result.Success(response);
    }
}
