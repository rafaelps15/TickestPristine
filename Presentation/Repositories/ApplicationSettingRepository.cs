//using Microsoft.EntityFrameworkCore;
//using Tickest.Domain.Entities;
//using Tickest.Domain.Interfaces.Repositories;
//using Tickest.Persistence.Data;

//namespace Tickest.Persistence.Repositories;

//internal class ApplicationSettingRepository : BaseRepository<ApplicationSetting>,IApplicationSettingRepository
//{
   
//    public ApplicationSettingRepository(TickestContext context) : base(context)
//    {
       
//    }

//    public async Task<ApplicationSetting> GetSettingAsync(string key)
//    {
//        return await _context.ApplicationSettings
//                             .FirstOrDefaultAsync(s => s.Key == key);
//    }

//    public async Task SetSettingAsync(ApplicationSetting setting)
//    {
//        var existingSetting = await _context.ApplicationSettings
//                                               .FirstOrDefaultAsync(s => s.Key == setting.Key);

//        await AddAsync(setting);
//    }

//    public async Task UptadeSettingFlagAsync(string settingKey, string value)
//    {
//        var setting = await _context.ApplicationSettings
//            .FirstOrDefaultAsync(s => s.Key == settingKey);

//        //altera o valor da configuração, 
//        setting.Value = value;

//        await SaveChangesAsync();
//    }
//}
