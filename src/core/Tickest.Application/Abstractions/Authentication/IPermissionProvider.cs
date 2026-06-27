namespace Tickest.Application.Abstractions.Authentication;

public interface IPermissionProvider
{
    Task<bool> CanUserLoginAsync(Guid userId);

    Task<HashSet<string>> GetForUserIdAsync(Guid userId);

    Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId);

    HashSet<string> GetPermissionsForRole(string roleName);

    Task<bool> UserHasPermissionAsync(Guid userId, string permission);

    Task ValidatePermissionAsync(Guid userId, string permission);
}
