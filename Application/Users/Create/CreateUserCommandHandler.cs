using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Application.Users.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    #region Dependencies

    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly EncryptionHelper _encryptionHelper;
    //private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly TickestContext _context;
    private readonly IPermissionProvider _permissionProvider;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IPermissionRepository _permissionRepository;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IConfiguration configuration,
        //IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IPermissionRepository permissionRepository,
        ILogger<CreateUserCommandHandler> logger,
        IValidator<CreateUserCommand> validator,
        TickestContext context,
        IPermissionProvider permissionProvider,
        ISpecialtyRepository specialtyRepository)
        => (_userRepository, /*_roleRepository,*/ _passwordHasher, _permissionRepository, _logger, _validator, _context, _permissionProvider, _specialtyRepository) =
            (userRepository, /*roleRepository,*/ passwordHasher, permissionRepository, logger, validator, context, permissionProvider, specialtyRepository);

    #endregion

    #region Handle

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validar dados de entrada
        await ValidateRequestAsync(request, cancellationToken);

        // Criar o usuário
        var user = await CreateUserAsync(request, cancellationToken);

        // Atribuir roles, especialidades, setor, departamento e área
        await AssignRolesAndSpecialtiesAsync(user, request, cancellationToken);
        await AssignSectorDepartmentAndAreaAsync(user, request, cancellationToken);

        // Salvar as alterações no banco de dados
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Usuário {Email} criado com sucesso.", request.Email);
        return MapResponse(user);
    }

    #endregion

    #region Validation

    private async Task ValidateRequestAsync(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validação falhou para o e-mail: {Email}", request.Email);
            throw new ValidationException(validationResult.Errors);
        }

        var emailExists = await _userRepository.DoesEmailExistAsync(request.Email, cancellationToken);
        if (emailExists)
        {
            _logger.LogWarning("Tentativa de cadastro com e-mail já existente: {Email}", request.Email);
            throw new TickestException("O e-mail informado já está cadastrado.");
        }
    }

    #endregion

    #region User Creation

    private async Task<User> CreateUserAsync(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var salt = EncryptionHelper.CreateSaltaKey(32);
        var passwordHash = EncryptionHelper.CreatePasswordHash(request.Password, salt);

        var isFirstUser = !(await _userRepository.AnyUsersExistAsync(cancellationToken));

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.Trim(),
            Name = request.Name.Trim(),
            PasswordHas = passwordHash,
            Salt = salt,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        // Atribuindo role "AdminMaster" ao primeiro usuário, se necessário
        if (isFirstUser)
        {
            await AssignFirstUserRoleAsync(user, cancellationToken);
        }

        await _userRepository.AddAsync(user, cancellationToken);
        return user;
    }

    private async Task AssignFirstUserRoleAsync(User user, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetFirstRoleByNamesAsync(new[] { "AdminMaster" }, cancellationToken);
        if (role != null)
        {
            user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
        }
    }

    #endregion

    #region Roles and Specialties

    private async Task AssignRolesAndSpecialtiesAsync(User user, CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Atribuindo roles ao usuário
        var roles = await _roleRepository.GetAllRolesByNamesAsync(request.RoleNames, cancellationToken);
        if (roles == null || !roles.Any())
        {
            throw new TickestException("Nenhuma role válida encontrada.");
        }

        foreach (var role in roles)
        {
            if (!user.UserRoles.Any(ur => ur.RoleId == role.Id))
            {
                user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                await AssignPermissionsToUserAsync(user, role, cancellationToken);
            }
        }

        // Atribuindo especialidades ao usuário
        if (request.SpecialtyNames != null && request.SpecialtyNames.Any())
        {
            var specialties = await _specialtyRepository.GetSpecialtiesByNamesAsync(request.SpecialtyNames, cancellationToken);
            if (specialties == null || !specialties.Any())
            {
                throw new TickestException("Nenhuma especialidade válida encontrada.");
            }

            foreach (var specialty in specialties)
            {
                if (!user.UserSpecialties.Any(us => us.SpecialtyId == specialty.Id))
                {
                    user.UserSpecialties.Add(new UserSpecialty { UserId = user.Id, SpecialtyId = specialty.Id });
                }
            }
        }
    }

    private async Task AssignPermissionsToUserAsync(User user, Role role, CancellationToken cancellationToken)
    {
        // Obtendo os nomes das permissões para o papel
        var permissionNames = _permissionProvider.GetPermissionsForRole(role.Name);

        // Buscar as instâncias de Permission usando os nomes
        var permissions = await _permissionRepository.GetPermissionsByNamesAsync(permissionNames, cancellationToken);

        // Adicionar as permissões ao usuário
        foreach (var permission in permissions)
        {
            user.UserPermissions.Add(new UserPermission
            {
                UserId = user.Id,
                Permission = permission
            });
        }
    }


    #endregion

    #region Sector, Department, and Area Assignment

    private async Task AssignSectorDepartmentAndAreaAsync(User user, CreateUserCommand request, CancellationToken cancellationToken)
    {
        await AssignEntityAsync(request.SectorId, _context.Sectors, sector => user.UserSector = sector, "Setor inválido.", cancellationToken);
        await AssignEntityAsync(request.DepartmentId, _context.Departments, department => user.UserDepartment = department, "Departamento inválido.", cancellationToken);
        await AssignEntityAsync(request.AreaId, _context.Areas, area => user.UserArea = area, "Área inválida.", cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task AssignEntityAsync<T>(
      Guid? entityId,
      DbSet<T> dbSet,
      Action<T> setValueAction,
      string errorMessage,
      CancellationToken cancellationToken) where T : class
    {
        if (entityId.HasValue)
        {
            // Buscar a entidade com base no ID
            var entity = await dbSet.FindAsync(new object[] { entityId.Value }, cancellationToken);

            if (entity != null)
            {
                // Se a entidade for encontrada, atribui ao campo do usuário
                setValueAction(entity);
            }
            else
            {
                // Caso a entidade não seja encontrada, lança uma exceção ou retorna um erro
                throw new InvalidOperationException(errorMessage);
            }
        }
        else
        {
            // Se o ID não for fornecido (nulo), você pode lançar uma exceção ou tratar da maneira que achar melhor
            throw new ArgumentNullException(nameof(entityId), "O ID da entidade não pode ser nulo.");
        }
    }

    private CreateUserResponse MapResponse(User user) =>
           new CreateUserResponse(user.Id, user.Email, user.Name);

  
    #endregion
}