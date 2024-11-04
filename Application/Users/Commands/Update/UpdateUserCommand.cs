using MediatR;
using Tickest.Application.Interfaces;
using Tickest.Application.Validators;
using Tickest.Domain.Contracts.Responses.UserResponses;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.Commands.UpdateUserCommand;

public class UpdateUserCommand : IRequest<UpdateUserResponse>, ICommandValidator
{
    public int UsuerId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }

    public void Validate()
    {
        var validationResult = new UpdateUserValidator().Validate(this);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new TickestException(errorMessage);
        }
    }
}
