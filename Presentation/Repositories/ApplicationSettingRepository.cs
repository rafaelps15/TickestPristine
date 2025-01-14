using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class ApplicationSettingRepository : IApplicationSettingRepository
{
    private readonly TickestContext _context;

    public ApplicationSettingRepository(TickestContext context)
    {
        _context = context;
    }

    public async Task<ApplicationSetting> GetSettingAsync(string key)
    {
        return await _context.ApplicationSettings
                             .FirstOrDefaultAsync(s => s.Key == key);
    }

    public async Task SetSettingAsync(ApplicationSetting setting)
    {
        var existingSetting = await _context.ApplicationSettings
                                               .FirstOrDefaultAsync(s => s.Key == setting.Key);

        _context.ApplicationSettings.Add(setting);
        await _context.SaveChangesAsync();
    }
}
