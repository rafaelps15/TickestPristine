namespace Tickest.Application.Features.Departments.Get;

public sealed record DepartmentResponse(
    Guid Id,
    string Name,
    string Description,
    string ResponsibleUserName,
    string SectorName
);

