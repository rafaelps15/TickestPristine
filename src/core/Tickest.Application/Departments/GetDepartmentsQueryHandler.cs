using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Departments;

internal sealed class GetDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    : IQueryHandler<GetDepartmentsQuery, List<DepartmentResponse>>
{
    public async Task<Result<List<DepartmentResponse>>> Handle(GetDepartmentsQuery query, CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAllAsync(cancellationToken: cancellationToken);

        if (!departments.Any())
        {
            throw new TickestException("Nenhum departamento encontrado.");
        }

        var response = departments.Select(department => new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            ResponsibleUserName = department.ResponsibleUser?.Name ?? "Nenhum responsavel atribuido",
            SectorNames = department.Sectors?.Select(sector => sector.Name).ToList() ?? new List<string>()
        }).ToList();

        return Result.Success(response);
    }
}
