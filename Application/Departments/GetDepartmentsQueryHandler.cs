using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Departments;

internal sealed class GetDepartmentsQueryHandler(IApplicationDbContext context, IDepartmentRepository departmentRepository)
    : IQueryHandler<GetDepartmentsQuery, List<DepartmentResponse>>
{
    public async Task<Result<List<DepartmentResponse>>> Handle(GetDepartmentsQuery query, CancellationToken cancellationToken)
    {
        // Recupera todos os departamentos 
        var departments = await departmentRepository.GetAllAsync();

        if (departments == null || !departments.Any())
        {
            throw new TickestException("Nenhum departamento encontrado.");
        }

        // Mapeia os departamentos para DTO
        var response = departments.Select(department => new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            ResponsibleUserName = department.ResponsibleUser?.Name ?? "Nenhum responsável atribúido",
            SectorNames = department.Sectors?.Select(s => s.Name).ToList() ?? new List<string>()
        }).ToList();

        return Result.Success(response);
    }
}
