using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Tickest.Application.Interfaces;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommand : IRequest<CriarUsuarioResponse>, ICommandValidator
{
	#region Properties

	public Guid Id { get; set; }
	public string Email { get; set; }
	public string Senha { get; set; }
	public string Nome { get; set; }

	#endregion

	#region Validation Methods
	public void Validate()
	{
		ValidateEmail();
		ValidadeNome();
		ValidateSenha();
		SenhaAtendeCritérios();
	}

	public void ValidadeAtualizacao()
	{
		if (Id == Guid.Empty)
			throw new TickestException("O ID do usuário deve ser informado para a atualização.");

		ValidateEmail();
		ValidadeNome();

		if (!string.IsNullOrWhiteSpace(Senha))
		{
			ValidateSenha();
		}
	}

	#endregion

	#region Email and Passowrd Validation
	private void ValidateEmail()
	{
		if (string.IsNullOrWhiteSpace(Email) || !MailAddress.TryCreate(Email, out _))
			throw new TickestException("Email inválido.");
	}

	private void ValidadeNome()
	{
		if (string.IsNullOrWhiteSpace(Nome) || Nome.Length > 100)
			throw new TickestException("O nome é obrigatório e não pode ter mais de 100 caracteres.");
	}

	private void ValidateSenha()
	{
		if (!SenhaAtendeCritérios())
			throw new TickestException("A senha deve ter pelo menos 8 caracteres, incluir pelo menos uma letra maiúscula e dois caracteres especiais.");
	}

	private bool SenhaAtendeCritérios()
	{
		var senhaRegex = new Regex(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$");
		var especialCount = Senha.Count(c => "!@#$%^&*()_+-=[]{};':\"|,.<>/?".Contains(c));

		return senhaRegex.IsMatch(Senha) && especialCount >= 2;
	}
	#endregion
}
