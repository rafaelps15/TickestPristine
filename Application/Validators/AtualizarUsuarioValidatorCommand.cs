using FluentValidation;
using System.Net.Mail;
using Tickest.Application.Users.AtualizarUsuario;

namespace Tickest.Application.Validators;

public class AtualizarUsuarioValidatorCommand : AbstractValidator<AtualizarUsuarioCommand>
{
	public AtualizarUsuarioValidatorCommand()
	{
		// Validação do Id
		RuleFor(x => x.UsuarioId)
			.NotEmpty().WithMessage("O ID do usuário deve ser informado para a atualização.");

		// Validação do Email
		RuleFor(x => x.Email)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Email é obrigatório.")
			.Must(BeAValidEmail).WithMessage("Email inválido.");

		// Validação do Nome
		RuleFor(x => x.Nome)
			.NotEmpty().WithMessage("O nome é obrigatório.")
			.MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

		// Validação condicional da Senha
		RuleFor(x => x.Senha)
			.Cascade(CascadeMode.Stop)
			.MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
			.When(x => !string.IsNullOrWhiteSpace(x.Senha))
			.Must(ContainUpperCase).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
			.Must(ContainSpecialCharacters).WithMessage("A senha deve conter pelo menos dois caracteres especiais.")
			.When(x => !string.IsNullOrWhiteSpace(x.Senha));
	}

	private bool BeAValidEmail(string email)
	{
		try
		{
			var mailAddress = new MailAddress(email);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private bool ContainUpperCase(string senha)
	{
		return senha.Any(char.IsUpper);
	}

	private bool ContainSpecialCharacters(string senha)
	{
		return senha.Count(c => char.IsPunctuation(c) || char.IsSymbol(c)) >= 2;
	}
}
