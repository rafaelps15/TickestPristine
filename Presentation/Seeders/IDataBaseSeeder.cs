using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public interface IDatabaseSeeder
{
    Task SeedAsync(TickestContext context, CancellationToken cancellationToken = default);
}
