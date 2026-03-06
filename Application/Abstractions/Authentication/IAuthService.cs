namespace Tickest.Application.Abstractions.Authentication;

public interface IAuthService
{
    Guid GetCurrentUserId();
    IEnumerable<string> GetUserRoles();
    bool IsInRole(string role);
}
