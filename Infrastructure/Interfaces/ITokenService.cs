namespace Tickest.Infrastructure.Interfaces;

public interface ITokenService
{
    Task<string> GerarTokenAsync(string userId);
    Task<string> RenovarTokenAsync(string refreshToken);
}
