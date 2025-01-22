using Tickest.Persistence.Data;

namespace Tickest.Persistence.Helpers;

public static class SeederHelper
{
    public static async Task SeedEntityIfNotExistAsync<T>(TickestContext context,Func<Task> entitySeeder) where T : class
    {
        if (!context.Set<T>().Any())
        {
            await entitySeeder();
        }
    }
}