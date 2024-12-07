using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Create;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    IReadOnlyList<string> RoleNames,
    IReadOnlyList<string> SpecialtyNames,
    Guid? SectorId,
    Guid? DepartmentId,
    Guid? AreaId
) : ICommand<Guid>; 
