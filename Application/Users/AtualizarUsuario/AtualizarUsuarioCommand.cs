using MediatR;
using Tickest.Application.Interfaces;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Exceptions;
using Tickest.Application.Validators;
using FluentValidation;
using Tickest.Application.Users.CriarUsuario;

namespace Tickest.Application.Users.AtualizarUsuario;

public class AtualizarUsuarioCommand : IRequest<CriarUsuarioResponse>, ICommandValidator
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
		var validator = new AtualizarUsuarioValidatorCommand();
		var validationResult = validator.Validate(this);

		if (!validationResult.IsValid)
		{
			var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
			throw new TickestException(errorMessage);
		}
	}

	#endregion
}
