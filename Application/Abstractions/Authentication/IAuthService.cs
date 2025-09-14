namespace Tickest.Application.Abstractions.Authentication;

public interface IAuthService
{
    Guid GetCurrentUserId();
    string GetCurrentUserEmail();   
}
