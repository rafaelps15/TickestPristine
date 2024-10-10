using FluentValidation;
using System.Net.Mail;
using Tickest.Application.Users.CriarUsuario;

namespace Tickest.Application.Validators;

public class CriarUsuarioValidatorCommand : AbstractValidator<CriarUsuarioCommand>
{
	public CriarUsuarioValidatorCommand()
	{
		// Validação do Email
		RuleFor(x => x.Email)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Email é obrigatório.")
			.Must(BeAValidEmail).WithMessage("Email inválido.");

		// Validação do Nome
		RuleFor(x => x.Nome)
			.NotEmpty().WithMessage("O nome é obrigatório.")
			.MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

		// Validação da Senha
		RuleFor(x => x.Senha)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Senha é obrigatória.")
			.MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
			.Must(ContainUpperCase).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
			.Must(ContainSpecialCharacters).WithMessage("A senha deve conter pelo menos dois caracteres especiais.");
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
