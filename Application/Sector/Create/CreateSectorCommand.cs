using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Sectors.Create;

public record CreateSectorCommand(
    string Name,
    string Description,
    Guid? SectorManagerId
    //ICollection<CreateDepartmentCommand> Departments
) : ICommand<Guid>;

