using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Application.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IPermissionProvider permissionProvider, 
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        #region Validação do Usuário Existente

        var existingUser = await userRepository.GetUserByEmailAsync(command.Email, cancellationToken);

        if (existingUser != null)
        {
            logger.LogError("O e-mail {Email} já está em uso.", command.Email);
            throw new TickestException("O e-mail fornecido já está em uso.");
        }

        #endregion

        #region Obter Papéis Disponíveis

        // Obtém os papéis disponíveis usando o PermissionProvider
        var availableRoles = permissionProvider.GetAvailableRoles();

        // Verifica se o papel fornecido existe entre os papéis disponíveis
        if (!availableRoles.Contains(command.Role))
        {
            logger.LogError("O papel fornecido {Role} não existe.", command.Role);
            throw new TickestException("O papel fornecido não é válido.");
        }

        #endregion

        #region Criação do Novo Usuário

        var roleEntity = await roleRepository.GetRoleByNameAsync(command.Role, cancellationToken);

        if (roleEntity == null)
        {
            logger.LogError("O papel fornecido {Role} não existe.", command.Role);
            throw new TickestException("O papel fornecido não é válido.");
        }

        var (passwordHash, salt) = passwordHasher.HashWithSalt(command.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Name = command.Name,
            PasswordHash = passwordHash,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            Role = roleEntity // Define o papel do novo usuário
        };

        // Obtém as permissões com base no papel do usuário
        var permissions = permissionProvider.GetPermissionsForRole(command.Role);

        // Adicionar as permissões ao usuário (se necessário, caso o modelo de usuário suporte e necessite)
       
        logger.LogInformation("Usuário preparado para persistência: {UserId}.", user.Id);

        #endregion

        #region Persistência no Banco de Dados

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync();

        logger.LogInformation($"Usuário {user.Name} com o papel {user.Role} criado com sucesso.");

        #endregion

        #region Log de Auditoria

        // Caso seja necessário implementar um log de auditoria para criação de usuário, adicione aqui.

        //logger.LogInformation("Usuário {UserId} criado por {CreatorId} em {CreationTime}.", user.Id, currentUser.Id, DateTime.UtcNow);

        #endregion

        return Result.Success(user.Id);
    }
}
