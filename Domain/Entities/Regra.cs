namespace Tickest.Domain.Entities;

public class Regra : EntidadeBase
{
    public string Nome { get; set; }

    public ICollection<UsuarioRegra> UsuarioRegras { get; set; }
}
