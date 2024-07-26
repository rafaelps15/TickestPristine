using Tickest.Domain.Enum;

namespace Tickest.Domain.Entities;

public class Chamado : EntidadeBase
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }

    public ChamadoStatus Status { get; set; }
    public ChamadoPrioridade Prioridade { get; set; }

    public int SolicitanteId { get; set; }
    public Usuario Solicitante { get; set; } // QUEM SOLICITA

    public int AtendenteId { get; set; }
    public Usuario Atendente { get; set; } //QUEM CONVERSA COM SOLICITANTE E DIRECIONA PARA O SETOR CORRETO

    public int? AnalistaId { get; set; }
    public Usuario Analista { get; set; } //QUEM IRÁ RESOLVER DE FATO O PROBLEMA

    public int AreaId { get; set; }
    public Area Area { get; set; }

    public ICollection<Mensagem> Mensagens { get; set; }
}
