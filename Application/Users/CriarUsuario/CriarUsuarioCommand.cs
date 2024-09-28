using MediatR;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommand : IRequest<Unit>
{
    public string Email { get; set; }

    public string Senha { get; set; }

    public string Nome { get; set; } 
}
