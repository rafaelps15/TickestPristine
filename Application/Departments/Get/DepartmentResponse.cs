namespace Tickest.Application.Departments.Get;

public sealed record DepartmentResponse(
    Guid Id,
    string Name,
    string Description,
    string ResponsibleUserName,
    string SectorName
);

