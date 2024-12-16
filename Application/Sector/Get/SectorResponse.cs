namespace Tickest.Application.Sector.Get;

public sealed class SectorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ResponsibleUserName { get; set; }
    public string DepartmentName { get; set; }
    public List<string> AreaNames { get; set; }
}
