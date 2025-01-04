namespace Tickest.Application.Sector.Get;

public sealed record SectorResponse(
     Guid Id,
     string Name,
     string Description,
     string ResponsibleUserName,
     string DepartmentName
);