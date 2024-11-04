using MediatR;
using Tickest.Application.Interfaces;
using Tickest.Application.Validators;
using Tickest.Domain.Contracts.Responses.UserResponses;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.Commands.DeleteUserCommand;

public class DeleteUserCommand : IRequest<DeleteUserResponse>, ICommandValidator
{
    public int UserId { get; set; }

    public void Validate()
    {
        // Usando o DeleteUserValidator para validar o comando
        var validationResult = new DeleteUserValidator().Validate(this);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new TickestException(errorMessage);
        }
    }
}
