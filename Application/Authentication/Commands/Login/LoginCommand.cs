using MediatR;
using Tickest.Application.Validators;
using Tickest.Domain.Contracts.Responses.UserResponses;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }

    public void validate()
    {
        var validationResult = new LoginUserValidator().Validate(this);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new TimeoutException(errorMessage);
        }
    }
}
