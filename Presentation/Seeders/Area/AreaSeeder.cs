using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public static class AreaSeeder
{
    public static async Task SeedAreasAsync(
        TickestContext context,
        IAreaRepository areaRepository,
        IDepartmentRepository departmentRepository)
    {
        await AddAreasAync(areaRepository, departmentRepository);
    }

    private static async Task AddAreasAync(IAreaRepository areaRepository, IDepartmentRepository departmentRepository)
    {
        var departments = await departmentRepository.GetAllAsync();

        var fixedAreas = new List<Area>
        {
            //// Setor Operações - Departamento Gestão de Processos
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Mapeamento de Processos",
            //    Description = "Área responsável pelo mapeamento e análise de processos",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Melhoria Contínua",
            //    Description = "Responsável por implementar melhorias nos processos operacionais",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Riscos Operacionais",
            //    Description = "Área que gerencia e mitiga riscos operacionais",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Qualidade",
            //    Description = "Responsável pela garantia da qualidade dos processos",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},

            //// Setor Financeiro - Departamento Contabilidade
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Contabilidade Geral",
            //    Description = "Responsável pela contabilidade da empresa, controle de balancetes e auditoria financeira.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Contabilidade Fiscal",
            //    Description = "Responsável pela apuração de impostos e pelo cumprimento das obrigações fiscais.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},

            //// Setor Financeiro - Departamento Tesouraria
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Fluxo de Caixa",
            //    Description = "Responsável pelo controle de entradas e saídas de caixa, fluxo de caixa diário.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Investimentos",
            //    Description = "Responsável pela análise e aplicação de recursos financeiros da empresa.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},

            //// Setor Logística - Departamento Armazenamento
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Estoques",
            //    Description = "Responsável pelo controle de estoque, armazenagem e movimentação de mercadorias.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Armazém",
            //    Description = "Gerencia o layout do armazém, otimização do espaço e fluxo de materiais.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},

            //// Setor Logística - Departamento Distribuição
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Transporte",
            //    Description = "Responsável pela coordenação do transporte de mercadorias, análise de rotas e custos logísticos.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Distribuição e Expedição",
            //    Description = "Gerencia o processo de expedição e entrega das mercadorias aos clientes.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},

            // Setor Tecnologia - Departamento Desenvolvimento de Software
            new Area {
                Id = Guid.NewGuid(),
                Name = "Desenvolvimento Frontend",
                Description = "Desenvolve interfaces de usuário com tecnologias como HTML, CSS, JavaScript e frameworks (React, Angular, Vue.js).",
                DepartmentId = departments.Where(s => s.Name == "Setor Tecnologia").Select(s => s.Id).FirstOrDefault(),
                CreatedAt = DateTime.Now,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null
            },
            new Area {
                Id = Guid.NewGuid(),
                Name = "Desenvolvimento Backend",
                Description = "Desenvolve a lógica de servidor e APIs, gerenciando comunicação entre frontend e banco de dados.",
                CreatedAt = DateTime.Now,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null
            },
            new Area {
                Id = Guid.NewGuid(),
                Name = "Engenharia de Dados",
                Description = "Gerencia e organiza dados, criando pipelines e data warehouses para otimização do uso de dados.",
                CreatedAt = DateTime.Now,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null
            },
            new Area {
                Id = Guid.NewGuid(),
                Name = "Suporte Técnico",
                Description = "Oferece suporte técnico para desenvolvedores, configurando ambientes de desenvolvimento e integração.",
                CreatedAt = DateTime.Now,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null
            },

            //// Setor Tecnologia - Departamento Suporte Técnico
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Suporte de TI",
            //    Description = "Presta suporte técnico para sistemas e infraestrutura, garantindo a operação dos serviços de TI.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Manutenção de Sistemas",
            //    Description = "Realiza manutenção e atualização de sistemas, corrigindo bugs e aplicando melhorias.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //},
            //new Area {
            //    Id = Guid.NewGuid(),
            //    Name = "Gestão de Incidentes",
            //    Description = "Gerencia e resolve incidentes de TI, assegurando uma resposta rápida e eficaz.",
            //    CreatedAt = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    DeactivatedAt = null,
            //    UpdateAt = null
            //}
        };

        foreach (var area in fixedAreas)
        {
            await areaRepository.AddAsync(area);
        }
    }
}
