using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IPermissionProvider permissionProvider,
    IAuthService authService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        #region Validação do Usuário Existente

        var existingUser = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (existingUser != null)
        {
            logger.LogError("O e-mail {Email} já está em uso.", command.Email);
            throw new TickestException("O e-mail fornecido já está em uso.");
        }

        #endregion

        #region Validação de Permissões

        var hasUsers = await userRepository.AnyAsync(cancellationToken);

        if (!hasUsers)
        {
            if (command.Role != "AdminMaster")
            {
                throw new TickestException("O primeiro usuário do sistema deve ser AdminMaster.");
            }
        }
        else
        {
            var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
            var rolePermissions = permissionProvider.GetPermissionsForRole(currentUser.Role);

            if (!rolePermissions.Contains("ManageUsers"))
            {
                logger.LogError("Usuário {UserId} com papel {Role} não tem permissão para criar um novo usuário.", currentUser.Id, currentUser.Role);
                throw new TickestException("Você não tem permissão para criar um novo usuário.");
            }
        }

        #endregion

        #region Validação do Papel do Novo Usuário

        // Verifica se o papel fornecido é válido
        if (permissionProvider.GetPermissionsForRole(command.Role).Count == 0)
        {
            logger.LogError("Papel {Role} inválido para o novo usuário.", command.Role);
            throw new TickestException("Papel inválido.");
        }

        #endregion

        #region Criação do Novo Usuário

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Name = command.Name,
            PasswordHash = passwordHasher.Hash(command.Password),
            CreatedAt = DateTime.UtcNow,
            Role = command.Role
        };

        logger.LogInformation("Usuário preparado para persistência: {UserId}.", user.Id);

        #endregion

        #region Persistência no Banco de Dados

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Novo usuário criado com ID {UserId}.", user.Id);

        #endregion

        #region Log de Auditoria
        //Caso seja necessario criar auditoria irei implementar.

        //logger.LogInformation("Usuário {UserId} criado por {CreatorId} em {CreationTime}.", user.Id, currentUser.Id, DateTime.UtcNow);

        #endregion

        return Result.Success(user.Id);
    }
}
