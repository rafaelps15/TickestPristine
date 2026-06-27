using Microsoft.EntityFrameworkCore;
using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Departments;

internal sealed class GetDepartmentsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetDepartmentsQuery, List<DepartmentResponse>>
{
    public async Task<Result<List<DepartmentResponse>>> Handle(GetDepartmentsQuery query, CancellationToken cancellationToken)
    {
        var departments = await context.Departments
            .AsNoTracking()
            .Include(d => d.ResponsibleUser)
            .Include(d => d.Sectors)
            .Where(d => d.IsActive && !d.IsDeleted)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

        if (departments.Count == 0)
        {
            throw new TickestException("Nenhum departamento encontrado.");
        }

        var response = departments.Select(d => new DepartmentResponse
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            ResponsibleUserName = d.ResponsibleUser?.Name ?? "Nenhum responsável atribuído",
            SectorNames = d.Sectors.Select(s => s.Name).ToList()
        }).ToList();

        return Result.Success(response);
    }
}
