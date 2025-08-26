using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public class DatabaseSeeder
{
    private readonly IEnumerable<IDatabaseSeeder> _seeders;

    public DatabaseSeeder(IEnumerable<IDatabaseSeeder> seeders)
    {
        _seeders = seeders;
    }

    public async Task RunAsync(TickestContext context, CancellationToken cancellationToken = default)
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync(context, cancellationToken);
        }
    }
}
