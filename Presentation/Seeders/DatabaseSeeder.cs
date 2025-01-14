using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly TickestContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly RoleSeeder _roleSeeder;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        (TickestContext context, IPasswordHasher passwordHasher, RoleSeeder roleSeeder, ILogger<DatabaseSeeder> logger) dependencies)
    {
        _context = dependencies.context;
        _passwordHasher = dependencies.passwordHasher;
        _roleSeeder = dependencies.roleSeeder;
        _logger = dependencies.logger;
    }

    public void Seed()
    {
        try
        {
            _roleSeeder.SeedRoles();
            _logger.LogInformation("Banco de dados seedado com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Error ao seedar o banco de dados.");
        }
    }

    // Seeding de outros dados, como configurações, usuários padrão, etc.


    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!_context.Users.Any())
        {
            // Gerando GUIDs para Departamentos, Setores, Áreas e Especialidades
            var departmentId = Guid.NewGuid();
            var sectorId = Guid.NewGuid();
            var areaId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();

            // Criando o salt e hash para a senha
            const string password = "@teste123";
            var salt = EncryptionHelper.CreateSaltKey(32);
            var passwordHash = EncryptionHelper.CreatePasswordHashWithSalt(password, salt);

            // Criando usuários com nomes sequenciais diretamente
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste1",
                    Email = "usuarioteste1@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "AdminMaster",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste2",
                    Email = "usuarioteste2@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "GeneralAdmin",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste3",
                    Email = "usuarioteste3@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "SectorAdmin",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste4",
                    Email = "usuarioteste4@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "AdminMaster",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste5",
                    Email = "usuarioteste5@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "AreaAdmin",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste6",
                    Email = "usuarioteste6@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "TicketManager",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste7",
                    Email = "usuarioteste7@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "Collaborator",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste8",
                    Email = "usuarioteste8@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "SupportAnalyst",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste9",
                    Email = "usuarioteste9@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "AreaManager",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste10",
                    Email = "usuarioteste10@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "SectorManager",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                }
            };

            // Adicionando os usuários ao contexto
            _context.Users.AddRange(users);

            // Criação de Departamentos
            var department = new Department
            {
                Id = departmentId,
                Name = "Tecnologia da Informação",
                Description = "Departamento responsável por gerenciar TI",
                DepartmentManagerId = users.First(u => u.Name == "UsuarioTeste4").Id,
                CreatedAt = DateTime.Now
            };

            _context.Departments.Add(department);

            // Criação de Setores
            var sector = new Sector
            {
                Id = sectorId,
                Name = "Desenvolvimento",
                Description = "Setor de Desenvolvimento de Software",
                SectorManagerId = users.First(u => u.Name == "UsuarioTeste3").Id,
                CreatedAt = DateTime.Now
            };

            _context.Sectors.Add(sector);

            // Criação de Áreas
            var area = new Area
            {
                Id = areaId,
                Name = "Desenvolvimento Backend",
                Description = "Foco no desenvolvimento de APIs",
                DepartmentId = departmentId,
                AreaManagerId = users.First(u => u.Name == "UsuarioTeste5").Id,
                CreatedAt = DateTime.Now
            };

            _context.Areas.Add(area);

            // Criação de Especialidades
            var specialty1 = new Specialty
            {
                Id = specialtyId,
                Name = "ASP.NET Core",
                Description = "Desenvolvimento utilizando o framework ASP.NET Core para construir APIs robustas."
            };

            var specialty2 = new Specialty
            {
                Id = Guid.NewGuid(),
                Name = "Front End",
                Description = "Foco no desenvolvimento de interfaces de usuário com HTML, CSS e JavaScript."
            };

            // Adicionando as especialidades
            _context.Specialties.Add(specialty1);
            _context.Specialties.Add(specialty2);

            // Relacionando área com usuários
            area.Users = new List<User> { users.First(u => u.Name == "UsuarioTeste5") };

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}