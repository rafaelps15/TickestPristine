using System.Linq;
using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Departments.Get;

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
        var response = departments.Select(department => new DepartmentResponse(
            department.Id,
            department.Name,
            department.Description,
            department.DepartmentManager?.Name ?? "Nenhum responsável atribúido",
            department.Sector?.Name ?? "Nenhum setor atribuído" 
        )).ToList();

        return Result.Success(response);
    }
}
