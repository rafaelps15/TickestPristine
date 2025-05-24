using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IAuthService authService,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IPermissionProvider permissionProvider,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "CreateUser");

        logger.LogInformation("Usuário {UserId} autorizado para criar.", currentUser.Id);

        #endregion

        #region Validação do Usuário Existente

        if (await userRepository.GetUserByEmailAsync(command.Email, cancellationToken) is not null)
        {
            logger.LogError("O e-mail {Email} já está em uso.", command.Email);
            throw new TickestException("O e-mail fornecido já está em uso.");
        }

        #endregion

        #region Obter e Validar Papel

        var role = await roleRepository.GetByIdAsync(command.RoleId);
        if (role == null)
        {
            logger.LogError("O papel fornecido {RoleId} não existe.", command.RoleId);
            throw new TickestException("O papel fornecido não é válido.");
        }

        #endregion

        #region Obter Permissões do Papel

        // Obtém as permissões com base no papel do usuário
        var permissions = permissionProvider.GetPermissionsForRole(role.Name);

        // (Aqui você pode adicionar a lógica para validar se o papel tem permissões adequadas, caso necessário)

        logger.LogInformation("Permissões obtidas para o papel {RoleName}: {Permissions}.", role.Name, string.Join(", ", permissions));

        #endregion

        #region Criação do Novo Usuário

        var salt = EncryptionHelper.CreateSaltKey(command.Password.Length);
        var password = EncryptionHelper.CreatePasswordHashWithSalt(command.Password, salt);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Name = command.Name,
            PasswordHash = password,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            Role = role,
            IsActive = true
        };

        logger.LogInformation("Usuário preparado para persistência: {UserId}.", user.Id);

        #endregion

        #region Persistência no Banco de Dados

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync();

        logger.LogInformation("Usuário {UserId} criado com sucesso com o papel {Role}.", user.Id, role.Name);

        #endregion

        return Result.Success(user.Id);
    }
}
