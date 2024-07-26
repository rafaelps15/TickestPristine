namespace Tickest.Domain.Entities;

public class UsuarioRegra : EntidadeBase
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int RegraId { get; set; }
    public Regra Regra { get; set; }
}
