using Microsoft.EntityFrameworkCore;
using Tickest.Application.Abstractions.ReadServices;
using Tickest.Persistence.Data;
using Tickest.SharedKernel;

namespace Tickest.Persistence.ReadServices;

internal sealed class UserRegistrationReadService(ApplicationDbContext context) : IUserRegistrationReadService
{
    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public Task<bool> EmployeeCodeExistsAsync(string employeeCode, CancellationToken cancellationToken)
    {
        return context.Users.AnyAsync(u => u.EmployeeCode == employeeCode, cancellationToken);
    }

    public Task<EntityId?> GetActiveRoleIdByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return context.Roles
            .Where(r => r.Name == roleName && r.IsActive && !r.IsDeleted)
            .Select(r => r.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ActiveSectorExistsAsync(EntityId sectorId, CancellationToken cancellationToken)
    {
        return context.Sectors.AnyAsync(s => s.Id == sectorId && s.IsActive && !s.IsDeleted, cancellationToken);
    }

    public async Task<bool> ActiveSpecialtiesExistAsync(IReadOnlyCollection<EntityId> specialtyIds, CancellationToken cancellationToken)
    {
        var existingSpecialtyCount = await context.Specialties
            .CountAsync(s => specialtyIds.Contains(s.Id) && s.IsActive && !s.IsDeleted, cancellationToken);

        return existingSpecialtyCount == specialtyIds.Count;
    }
}
