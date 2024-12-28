using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Login;

public record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<String>;
