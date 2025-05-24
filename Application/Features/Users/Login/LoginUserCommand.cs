using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Users.Login;

public record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<string>;
