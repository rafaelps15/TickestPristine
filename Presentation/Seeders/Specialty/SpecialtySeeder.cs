using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public static class SpecialtySeeder
{
    public static async Task SeedSpecialtiesAsync(
        TickestContext context,
        ISpecialtyRepository specialtyRepository,
        IAreaRepository areaRepository)
    {
        await AddSpecialtyAsync(context, specialtyRepository, areaRepository);
    }

    private static async Task AddSpecialtyAsync(TickestContext context, ISpecialtyRepository specialtyRepository, IAreaRepository areaRepository)
    {
        var areas = await areaRepository.GetAllAsync();

        var fixedSpecialty = new List<Specialty>
        {
            new Specialty
            {
                Id = new Guid(),
                Name = "Frontend Developer",
                Description = "Especialista no desenvolvimento de interfaces de usuário com tecnologias como HTML, CSS, JavaScript e frameworks como React, Angular ou Vue.js.",
                AreaId = areas.Where(a => a.Name == "Desenvolvimento Backend").Select(a => a.Id).FirstOrDefault(),
                DeactivatedAt = DateTime.Now,
            },
            new Specialty
            {
                Id = new Guid(),
                Name = "Backend Developer",
                Description ="Especialista no desenvolvimento da lógica de servidor, APIs e gerenciamento de comunicação entre frontend e banco de dados.",
                AreaId = areas.Where(a => a.Name == "Desenvolvimento Backend").Select(a => a.Id).FirstOrDefault(),
                DeactivatedAt = DateTime.Now,
            },
            new Specialty
            {
                Id = new Guid(),
                Name = "Data Engineer",
                Description = "Especialista em engenharia de dados, criando pipelines e data warehouses para otimização do uso de dados.",
                AreaId = areas.Where(a => a.Name == "Engenharia de Dados" ).Select(a => a.Id).FirstOrDefault(),
                DeactivatedAt = DateTime.Now,

            },
            new Specialty
            {
                Id = new Guid(),
                Name = "Technical Support",
                Description = "Especialista em suporte técnico para desenvolvedores, configurando ambientes de desenvolvimento e integração.",
                AreaId = areas.Where(a => a.Name == "Suporte Técnico").Select(a => a.Id).FirstOrDefault(),
                DeactivatedAt = DateTime.Now,
            }
        };

        foreach (var specialty in fixedSpecialty)
        {
            await specialtyRepository.AddAsync(specialty);
        }
    }
}
