using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IApplicationSettingRepository : IBaseRepository<ApplicationSetting>
{
    Task<ApplicationSetting> GetSettingAsync(string key);
    Task SetSettingAsync(ApplicationSetting setting);
    Task UptadeSettingFlagAsync(string settingKey, string value);
}
