using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Persistence.Data;
using Tickest.Persistence.Repositories;

namespace Tickest.Persistence.Seeders;

public static class UserSeeder
{
    public static async Task SeedUsersAsync(
        TickestContext context,
        RoleRepository roleRepository,
        UserRepository userRepository,
        CancellationToken cancellationToken = default)
    {
        var userRoles = new Dictionary<string, (string RoleName, string? Password)>
        {
            { "admin.master@tickest.com", ("AdminMaster", "admin@123") },
            { "admin.general@tickest.com", ("AdminGeneral", "admin@123") },
            { "sector.admin@tickest.com", ("SectorAdmin", null) },
            { "department.admin@tickest.com", ("DepartmentAdmin", null) },
            { "area.admin@tickest.com", ("AreaAdmin", null) },
            { "ticket.manager@tickest.com", ("TicketManager", null) },
            { "collaborator@tickest.com", ("Collaborator", null) }
        };

        foreach (var (email, (roleName, password)) in userRoles)
        {
            var role = await roleRepository.GetRoleByNameAsync(roleName, cancellationToken);
            if (role == null)
            {
                throw new TickestException($"Role '{roleName}' não encontrada no banco de dados.");
            }

            var user = await CreateUserAsync(email, role, password, cancellationToken);
            await userRepository.AddAsync(user);
        }

         await userRepository.SaveChangesAsync();
    }

    private static async Task<User> CreateUserAsync(string email, Role role, string? password, CancellationToken cancellationToken)
    {
        string? salt = null;
        string? passwordHash = null;

        if (!string.IsNullOrEmpty(password))
        {
            var (generatedSalt, generatedHash) = EncryptionHelper.GeneratePasswordHash(password);
            salt = generatedSalt;
            passwordHash = generatedHash;
        }

        return new User
        {
            Id = Guid.NewGuid(),
            Name = role.Name.Replace("Admin", "Administrador").Replace("Collaborator", "Colaborador"),
            Email = email,
            PasswordHash = passwordHash,
            Salt = salt,
            RoleId = role.Id,
            CreatedAt = DateTime.Now,
            IsActive = true
        };
    }
}
