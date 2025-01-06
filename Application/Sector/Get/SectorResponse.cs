namespace Tickest.Application.Departments.Get;

public sealed record SectorResponse(
     Guid Id,
     string Name,
     string Description,
     string ResponsibleUserName,
     string DepartmentName
);