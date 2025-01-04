using System.Collections.Generic;
using System.Threading.Tasks;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication
{
    /// <summary>
    /// Provides methods for handling permissions related to users and roles.
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Retrieves the permissions assigned to a specific user.
        /// This includes both individual permissions and those associated with the user's roles.
        /// </summary>
        /// <param name="user">The user whose permissions are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a set of permissions.</returns>
        Task<HashSet<string>> GetPermissionsForUserAsync(User user);

        /// <summary>
        /// Retrieves the permissions associated with a specific role.
        /// </summary>
        /// <param name="roleName">The name of the role whose permissions are to be retrieved.</param>
        /// <returns>A set of permissions associated with the role.</returns>
        HashSet<string> GetPermissionsForRole(string roleName);

        /// <summary>
        /// Checks if a user has a specific permission.
        /// </summary>
        /// <param name="user">The user to check the permission for.</param>
        /// <param name="permission">The permission to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user has the permission.</returns>
        Task<bool> UserHasPermissionAsync(User user, string permission);

        /// <summary>
        /// Validates if a user has a specific permission. 
        /// Throws an exception if the user does not have the permission.
        /// </summary>
        /// <param name="user">The user to check the permission for.</param>
        /// <param name="permission">The permission to validate.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ValidatePermissionAsync(User user, string permission);

        /// <summary>
        /// Retrieves a list of all available roles.
        /// </summary>
        /// <returns>A list of role names.</returns>
        List<string> GetAvailableRoles();
    }
}
