namespace Tickest.Application.Sector.Get;

public sealed class SectorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ResponsibleUserName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public List<string> AreaNames { get; set; } = [];
}
