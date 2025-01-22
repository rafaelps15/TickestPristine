using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.Persistence.Helpers;

namespace Tickest.Persistence.Seeders;

public class DatabaseSeeder
{
    private readonly TickestContext _context;
    private readonly (
        ISectorRepository sectorRepository,
        IPermissionRepository permissionRepository,
        IDepartmentRepository departmentRepository,
        IAreaRepository areaRepository,
        IRoleRepository roleRepository,
        ISpecialtyRepository specialtyRepository) _repositories;

    public DatabaseSeeder(
        TickestContext context,
        (
            ISectorRepository sectorRepository,
            IPermissionRepository permissionRepository,
            IDepartmentRepository departmentRepository,
            IAreaRepository areaRepository,
            IRoleRepository roleRepository,
            ISpecialtyRepository specialtyRepository
        ) repositories)
    {
        _context = context;
        _repositories = repositories;
    }

    public async Task SeedAsync()
    {
        await SeedPermissionsAsync();
        await SeedRolesAsync();
        await SeedSectorsAsync();
        await SeedDepartmentsAsync();
        await SeedAreasAsync();
        await SeedSpecialtyAsync(); // Adicionado para chamar SeedSpecialty
    }

    private async Task SeedPermissionsAsync()
    {
        await SeederHelper.SeedEntityIfNotExistAsync<Permission>(
            _context,
            async () => await PermissionSeeder.SeedPermissionsAsync(_context, _repositories.permissionRepository)
        );
    }

    private async Task SeedRolesAsync()
    {
        await SeederHelper.SeedEntityIfNotExistAsync<Role>(
            _context,
            async () => await RoleSeeder.SeedRolesAsync(_context, _repositories.roleRepository, _repositories.permissionRepository)
        );
    }

    private async Task SeedSectorsAsync()
    {
        await SeederHelper.SeedEntityIfNotExistAsync<Sector>(
            _context,
            async () => await SectorSeeder.SeedSectorsAsync(_context, _repositories.sectorRepository)
        );
    }

    private async Task SeedDepartmentsAsync()
    {
        await SeederHelper.SeedEntityIfNotExistAsync<Department>(
            _context,
            async () => await DepartmentSeeder.SeedDepartmentsAsync(_context, _repositories.departmentRepository, _repositories.sectorRepository)
        );
    }

    private async Task SeedAreasAsync()
    {
        await SeederHelper.SeedEntityIfNotExistAsync<Area>(
            _context,
            async () => await AreaSeeder.SeedAreasAsync(_context, _repositories.areaRepository, _repositories.departmentRepository)
        );
    }

    private async Task SeedSpecialtyAsync()
    {
        await SeederHelper.SeedEntityIfNotExistAsync<Specialty>(
            _context,
            async () => await SpecialtySeeder.SeedSpecialtiesAsync(_context, _repositories.specialtyRepository, _repositories.areaRepository)
        );
    }
}
