using FluentValidation;
using Tickest.Application.Users.Commands.DeleteUserCommand;
using Tickest.Domain.Exceptions; 

namespace Tickest.Application.Validators
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
                .GreaterThan(0).WithMessage("O ID do usuário deve ser maior que zero.");
        }

        public void ValidateRequest(DeleteUserCommand command)
        {
            var validationResult = Validate(command);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new TickestException(errorMessage);
            }
        }
    }
}
