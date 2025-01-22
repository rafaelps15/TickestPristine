using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public static class SectorSeeder
{
    public static async Task SeedSectorsAsync(
        TickestContext context,
        ISectorRepository sectorRepository)
    {
        await AddSectorsAsync(sectorRepository);
    }

    private static async Task AddSectorsAsync(ISectorRepository sectorRepository)
    {
        var fixedSectors = new List<Sector>
        {
            //new Sector {Name = "Administração Geral", Description = "Gestão de todos os departamentos e áreas da organização", CreatedAt = DateTime.Now},
            //new Sector {Name = "Operações", Description = "Setor responsável pelas operações principais da empresa", CreatedAt = DateTime.Now},
           
            new Sector {
                Name = "Tecnologia",
                Description = "Setor de Tecnologia da Informação e Inovação",
                CreatedAt = DateTime.Now ,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null},


            //new Sector {Name = "Financeiro", Description = "Gestão de finanças e recursos econômicos", CreatedAt = DateTime.Now},
            //new Sector {Name = "Logística", Description = "Gestão de suprimentos e transporte", CreatedAt = DateTime.Now}
        };

        foreach (var sector in fixedSectors)
        {
            await sectorRepository.AddAsync(sector);
        }
    }
}


