using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Helpers;

public static class SeederHelper
{
    public static async Task SeedEntityIfNotExistAsync<T>(
        TickestContext context,
        IApplicationSettingRepository applicationSettingRepository,
        string settingKey,
        Func<Task> entitySeeder) where T : class
    {
        var seederFlag = await applicationSettingRepository.GetSettingAsync(settingKey);
        if (seederFlag?.Value != "True")
        {
            await entitySeeder();
            await applicationSettingRepository.UptadeSettingFlagAsync(settingKey, "True");
        }
    }

}