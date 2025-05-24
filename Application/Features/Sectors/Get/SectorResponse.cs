namespace Tickest.Application.Features.Sectors.Get;

public sealed record SectorResponse(
     Guid Id,
     string Name,
     string Description,
     string ResponsibleUserName,
     string DepartmentName
);