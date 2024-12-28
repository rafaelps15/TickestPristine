using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Create;

public record RegisterUserCommand(
    string Name,
    string Email,
    string Password,
    string Role,
    IReadOnlyList<string> SpecialtyNames,
    IReadOnlyList<Guid> AreaIds
) : ICommand<Guid>
{ }


