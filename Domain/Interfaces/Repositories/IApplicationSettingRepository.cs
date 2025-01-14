using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IApplicationSettingRepository
{
    Task<ApplicationSetting> GetSettingAsync(string key);
    Task SetSettingAsync(ApplicationSetting setting);
}
