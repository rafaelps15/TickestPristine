namespace Tickest.Domain.Entities;

public class Area : EntidadeBase
{
    public string Nome { get; set; }

    public int SetorId { get; set; }
    public Setor Setor { get; set; }

    public ICollection<Usuario> Usuarios { get; set; }
    public ICollection<Chamado> Chamados { get; set; }
}
