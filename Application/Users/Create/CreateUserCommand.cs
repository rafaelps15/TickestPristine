using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Create;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    IReadOnlyList<string> SpecialtyNames,
    IReadOnlyList<Guid> AreaIds
) : ICommand<Guid>; 
