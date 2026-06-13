using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Create;

public record RegisterUserCommand(
    string Name,
    string EmployeeCode,
    string Email,
    string Password,
    Guid RoleId,
    Guid? SectorId = null,
    IReadOnlyCollection<Guid>? SpecialtyIds = null
) : ICommand<Guid>
{ }


