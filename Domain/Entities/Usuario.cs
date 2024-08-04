namespace Tickest.Domain.Entities;

public class Usuario : EntidadeBase
{ 
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }

    public int? AreaId { get; set; }
    public Area Area { get; set; }

    public ICollection<UsuarioRegra> UsuarioRegras { get; set; }
    public ICollection<Chamado> ChamadosSolicitados { get; set; } //meus chamados solicitados
    public ICollection<Chamado> ChamadosAtendentes { get; set; } // Chamados que ele é atendente
    public ICollection<Chamado> ChamadosAtendimentos { get; set; } // Chamados que eu sou analista
    public ICollection<Mensagem> Mensagens { get; set; }
}
