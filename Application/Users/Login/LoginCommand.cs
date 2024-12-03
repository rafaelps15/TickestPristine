using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Login;

public record LoginCommand(string Email, string Password ): ICommand<String>;
