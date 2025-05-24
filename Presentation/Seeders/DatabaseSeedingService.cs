using Microsoft.Extensions.DependencyInjection;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public static class DatabaseSeedingService
{
    public static async Task SeedDatabaseAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<TickestContext>();
        var seederRunner = provider.GetRequiredService<DatabaseSeederRunner>();

        await seederRunner.RunAsync(context, cancellationToken);
    }
}
