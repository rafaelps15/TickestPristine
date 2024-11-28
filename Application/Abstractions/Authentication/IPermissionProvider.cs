
namespace Tickest.Application.Abstractions.Authentication;

public interface IPermissionProvider
{
    //Task<HashSet<string>> GetForUserIdAsync(Guid userId);

    Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId);

    HashSet<string> GetPermissionsForRole(string roleName);
}