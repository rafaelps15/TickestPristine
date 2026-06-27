using Tickest.SharedKernel;

namespace Tickest.Application.Abstractions.ReadServices;

public interface IUserRegistrationReadService
{
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);

    Task<bool> EmployeeCodeExistsAsync(string employeeCode, CancellationToken cancellationToken);

    Task<EntityId?> GetActiveRoleIdByNameAsync(string roleName, CancellationToken cancellationToken);

    Task<bool> ActiveSectorExistsAsync(EntityId sectorId, CancellationToken cancellationToken);

    Task<bool> ActiveSpecialtiesExistAsync(IReadOnlyCollection<EntityId> specialtyIds, CancellationToken cancellationToken);
}
