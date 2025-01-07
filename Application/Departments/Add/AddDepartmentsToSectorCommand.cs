using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Departments.Add;

public record  AddDepartmentsToSectorCommand(
    Guid SectorId, 
    ICollection<Guid> DepartmentIds
) : ICommand<Guid>;
