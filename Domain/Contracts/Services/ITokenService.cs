namespace Tickest.Domain.Contracts.Services;

public interface ITokenService
{
    Task<string> GerarTokenAsync(string userId);
    Task<string> RenovarTokenAsync(string refreshToken);
}
