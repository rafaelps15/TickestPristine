namespace Tickest.Application.Sectors.Get;

public sealed class SectorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ResponsibleUserName { get; set; }
    public string? DepartmentName { get; set; }
}
