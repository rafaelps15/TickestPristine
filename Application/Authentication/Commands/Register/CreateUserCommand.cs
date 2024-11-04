using MediatR;
using Tickest.Application.Interfaces;
using Tickest.Application.Validators;
using Tickest.Domain.Contracts.Responses.UserResponses;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Authentication.Commands.Register;

public class CreateUserCommand : IRequest<CreateUserResponse>, ICommandValidator
{
    #region Properties

    public int Id { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    #endregion

    #region Method Validation

    public void Validate()
    {
        var validationResult = new CreateUserValidator().Validate(this);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new TickestException(errorMessage);
        }
    }

    #endregion
}
