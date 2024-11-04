namespace Tickest.Infrastructure.Configuracoes
{
    public class JwtConfiguracao
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        public double TokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public char[] Secret { get; internal set; }
    }
}
