using MediatR;
using Tickest.Application.Interfaces;
using Tickest.Application.Validators;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommand : IRequest<CriarUsuarioResponse>, ICommandValidator
{
    #region Properties
    public int Id { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Nome { get; set; }
    #endregion

    #region Validation Methods
    public void Validate()
    {
        var validator = new CriarUsuarioValidatorCommand(); 
        var validationResult = validator.Validate(this);

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new TickestException(errorMessage);
        }
    }
    #endregion
}
