using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;

namespace Tickest.Application.Users.Commands.Login;

public class LoginCommand : ICommand<LoginResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
