namespace Tickest.Application.Departments
{
    public sealed class DepartmentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ResponsibleUserName { get; set; } = string.Empty;
        public List<string> SectorNames { get; set; } = [];
    }
}
