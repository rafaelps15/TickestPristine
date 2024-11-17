namespace Tickest.Infrastructure.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpireMinutes { get; set; }
        public char[] SecretKey { get; internal set; }
        public double ExpirationInMinutes { get; internal set; }
        public double ExpirationMinutes { get; internal set; }
    }

}
