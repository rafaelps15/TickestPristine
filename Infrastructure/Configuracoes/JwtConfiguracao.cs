namespace Tickest.Infrastructure.Configuracoes;

public  class JwtConfiguracao
{
    public string ChaveSecreta { get; set; }
    public string Emissor { get; set; }
    public string Audiencia { get; set; }
    public int ExpiracaoEmMinutos { get; set; }
}
