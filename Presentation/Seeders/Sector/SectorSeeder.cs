using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders.Sector;

public class SectorSeeder
{
    private readonly TickestContext _context;
    private readonly ISectorRepository _sectorRepository;
    private readonly IApplicationSettingRepository _applicationSettingRepository;

    public SectorSeeder(TickestContext context, ISectorRepository sectorRepository, IApplicationSettingRepository applicationSettingRepository)
    {
        _context = context;
        _sectorRepository = sectorRepository;
        _applicationSettingRepository = applicationSettingRepository;
    }

    public async Task SeedSector()
    {
        var seederFlag = await _applicationSettingRepository.GetSettingAsync("SectorSeeded");

        if (seederFlag is null || seederFlag.Value != "true")
        {
            if (!_context.Sectors.Any())
            {

            }
        }
    }
}
