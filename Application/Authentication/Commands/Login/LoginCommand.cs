using MediatR;
using Tickest.Domain.Contracts.Models;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommand : IRequest<TokenModel>
{
    public string Email { get; set; }

    public string Senha { get; set; }
}
