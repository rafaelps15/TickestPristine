namespace Tickest.Domain.Entities;

public class Usuario : EntidadeBase
{ 
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }

    public int? AreaId { get; set; }
    public Area Area { get; set; }

    public ICollection<UsuarioRegra> UsuarioRegras { get; set; }
    public ICollection<Chamado> ChamadosAbertos { get; set; }
    public ICollection<Chamado> ChamadosAtendidos { get; set; }
    public ICollection<Mensagem> Mensagens { get; set; }
}
