using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Application.Features.Users.Login;

public record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<string>;
