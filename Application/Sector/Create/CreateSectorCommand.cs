using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Sectors.Create;

public record CreateSectorCommand(
    Guid Id,
    string Name,
    string Description,
    Guid? SectorManagerId,
    ICollection<CreateDepartmentCommand> Departments
) : ICommand<Guid>;

