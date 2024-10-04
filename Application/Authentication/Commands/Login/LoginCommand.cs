using MediatR;
using Tickest.Application.Interfaces;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommand : IRequest<TokenResponse>, ICommandValidator
{
    public string Email { get; set; }
    public string Senha { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
            throw new TickestException("Email e senha devem ser informados.");
    }
}
