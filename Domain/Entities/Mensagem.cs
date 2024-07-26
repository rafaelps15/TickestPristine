namespace Tickest.Domain.Entities;

public class Mensagem : EntidadeBase
{
    public string Conteudo { get; set; }
    public DateTime DataEnvio { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int ChamadoId { get; set; }
    public Chamado Chamado { get; set; }
}
