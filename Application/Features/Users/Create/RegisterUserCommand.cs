using System.Globalization;
using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Users.Create;

public record RegisterUserCommand(
    string Name,
    string Email,
    string Password,
    Guid RoleId
) : ICommand<Guid>
{ }


