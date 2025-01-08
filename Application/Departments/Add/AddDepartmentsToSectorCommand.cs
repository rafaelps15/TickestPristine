using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Departments.Add;

public record  AddDepartmentsToSectorCommand(
    Guid SectorId,
    Guid DepartamentId
) : ICommand<Guid>;
