using MediatR;
using Tickest.Domain.Contracts.Responses;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommand : IRequest<CriarUsuarioResponse>
{
	public Guid Id { get; set; }
	public string Email { get; set; }
	public string Senha { get; set; }
	public string Nome { get; set; }
}




