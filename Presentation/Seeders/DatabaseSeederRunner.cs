using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public class DatabaseSeederRunner
{
    private readonly IEnumerable<IDatabaseSeeder> _seeders;

    public DatabaseSeederRunner(IEnumerable<IDatabaseSeeder> seeders)
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
