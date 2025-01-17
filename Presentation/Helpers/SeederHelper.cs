using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Helpers;

public static class SeederHelper
{
    public static async Task SeedEntityIfNotExistAsync<T>(
        TickestContext context,
        IEnumerable<T> entities,
        IApplicationSettingRepository applicationSettingRepository,
        string settingKey) where T : class
    {
        // Verifica se a flag de seeding está definida como "True"
        var seederFlag = await applicationSettingRepository.GetSettingAsync(settingKey);

        /// <summary>
        /// Utilizado o <see cref="AddRangeAsync"/> e <see cref="SaveChangesAsync"/> do EF Core para 
        /// adicionar as entidades de forma eficiente, sem a necessidade de dados específicos para 
        /// a inserção no banco de dados, pois o processo será controlado manualmente.
        /// </summary>

        // Se a flag não for encontrada ou não estiver com valor "True", realiza o seeding
        if (seederFlag?.Value != "True")
        {
            await context.Set<T>().AddRangeAsync(entities);

            await context.SaveChangesAsync();

            await applicationSettingRepository.UptadeSettingFlagAsync(settingKey, "True");
        }

    }
}