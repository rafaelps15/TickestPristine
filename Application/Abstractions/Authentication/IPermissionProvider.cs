using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tickest.Application.Abstractions.Authentication
{
    public interface IPermissionProvider
    {
        /// <summary>
        /// Gets the set of permissions assigned to a user, either from their roles or directly assigned.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>A set of permissions assigned to the user.</returns>
        Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId);

        /// <summary>
        /// Gets the set of permissions associated with a specific role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>A set of permissions assigned to the role.</returns>
        HashSet<string> GetPermissionsForRole(string roleName);

        /// <summary>
        /// Checks if a user has a specific permission.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="permission">The permission to check.</param>
        /// <returns>A boolean indicating whether the user has the specified permission.</returns>
        bool UserHasPermission(Guid userId, string permission);
    }
}
