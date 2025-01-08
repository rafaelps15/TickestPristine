using FluentValidation;

namespace Tickest.Application.Users.Add
{
    public class AddUserToSectorCommandValidator : AbstractValidator<AddUserToSectorCommand>
    {
        public AddUserToSectorCommandValidator()
        {
            RuleFor(command => command.SectorId)
                .NotEmpty().WithMessage("O ID do setor não pode ser vazio.")
                .NotNull().WithMessage("O ID do setor não pode ser nulo.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("O ID do usuário não pode ser vazio.")
                .NotNull().WithMessage("O ID do usuário não pode ser nulo.");
        }
    }
}
