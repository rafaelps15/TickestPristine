using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

//VERIFICAR A POSSIBILIDADE DE MUDAR PARA DICTONARY AS LISTAS, DIMINUINDO A BUSCA REPETIDAS NO BDADOS.
public static class DepartmentSeeder
{
    public static async Task SeedDepartmentsAsync(
        TickestContext context,
        IDepartmentRepository departmentRepository,
        ISectorRepository sectorRepository)
    {
        await AddDepartmentsAsync(departmentRepository, sectorRepository);
    }

    private static async Task AddDepartmentsAsync(
        IDepartmentRepository departmentRepository,
        ISectorRepository sectorRepository)
    {

        var sectors = await sectorRepository.GetAllAsync();

        var fixedDepartments = new List<Department>
        {
            //// Setor Operações - Departamento Gestão de Processos
            //new Department
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Processos",
            //    Description = "Responsável pelo mapeamento e análise de processos",
            //    SectorId = sectors.Where(s => s.Name == "Operações").Select(s => s.Id).FirstOrDefault()
            //},

            //// Setor Operações - Departamento Qualidade
            //new Department
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Qualidade",
            //    Description = "Responsável pela garantia da qualidade dos processos",
            //    SectorId = sectors.Where(s => s.Name == "Operações").Select(s => s.Id).FirstOrDefault()
            //},

            // Setor Tecnologia - Departamento Desenvolvimento de Software
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Desenvolvimento de Software",
                Description = "Desenvolve interfaces de usuário e lógicas de servidor com tecnologias como HTML, CSS, JavaScript, React, Angular, Vue.js, e APIs.",
                SectorId = sectors.Where(s => s.Name == "Tecnologia").Select(s => s.Id).FirstOrDefault(),
                CreatedAt = DateTime.Now,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null
            },

            // Setor Tecnologia - Departamento Suporte Técnico
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Suporte Técnico",
                Description = "Oferece suporte técnico para desenvolvedores, configurando ambientes de desenvolvimento e integração.",
                SectorId = sectors.Where(s => s.Name == "Tecnologia").Select(s => s.Id).FirstOrDefault(),
                CreatedAt = DateTime.Now,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null
            },

            //// Setor Financeiro - Departamento Contabilidade
            //new Department
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Contabilidade",
            //    Description = "Área responsável pela contabilidade da empresa",
            //    SectorId = sectors.Where(s => s.Name == "Contabilidade").Select(s => s.Id).FirstOrDefault()
            //},

            //// Setor Financeiro - Departamento Tesouraria
            //new Department
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Tesouraria",
            //    Description = "Responsável pelo gerenciamento financeiro e fluxo de caixa da empresa",
            //    SectorId = sectors.Where(s => s.Name == "Tesouraria").Select(s => s.Id).FirstOrDefault()
            //},

            //// Setor Logística - Departamento Armazenamento
            //new Department
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Armazenamento",
            //    Description = "Responsável pela gestão de estoque e inventário",
            //    SectorId = sectors.Where(s => s.Name == "Armazenamento").Select(s => s.Id).FirstOrDefault()
            //},

            //// Setor Logística - Departamento Distribuição
            //new Department
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Distribuição",
            //    Description = "Responsável pelo planejamento das rotas de entrega e transporte de produtos",
            //    SectorId = sectors.Where(s => s.Name == "Distribuição").Select(s => s.Id).FirstOrDefault()
            //}
        };

        foreach (var department in fixedDepartments)
        {
            await departmentRepository.AddAsync(department);
        }
    }
}
