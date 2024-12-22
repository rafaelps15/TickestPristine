using System.Reflection.Metadata.Ecma335;

namespace Tickest.Infrastructure.Authentication;

public class JwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; } 
    public string Audience { get; set; } 
    public int ExpirationInMinutes { get;  set; }
}
