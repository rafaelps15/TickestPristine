using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickest.Domain.Interfaces;

public interface IDatabaseSeeder
{
    Task SeedAsync(CancellationToken cancellationToken);
}
