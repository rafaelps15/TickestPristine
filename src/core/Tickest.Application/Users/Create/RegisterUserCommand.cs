using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Create;

public record RegisterUserCommand(
    string Name,
    string Email,
    string Password,
    Guid RoleId
) : ICommand<Guid>
{ }


