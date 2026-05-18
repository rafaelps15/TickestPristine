namespace Tickest.Application.Area.Get;

public sealed class AreaResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SpecialtyName { get; set; } = string.Empty;
}
