namespace Tickest.Domain.Entities;

public class Setor : EntidadeBase
{
    public string Nome { get; set; }

    public int ResponsavelId { get; set; }
    public Usuario Responsavel { get; set; }

    public ICollection<Area> Areas { get; set; }
}
