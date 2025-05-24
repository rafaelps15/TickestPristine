using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Delete;

internal sealed class DeleteUserCommandHandler(
    IBaseRepository<User> userRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<DeleteUserCommandHandler> logger)
    : ICommandHandler<DeleteUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando solicitação de exclusão de usuário.");

        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "DeleteUser");

        logger.LogInformation("Usuário {UserId} autorizado para excluir.", currentUser.Id);

        #endregion

        #region Obtenção de Usuário

        var userToDelete = await userRepository.GetByIdAsync(request.UserId);

        if (userToDelete == null)
        {
            logger.LogError("Usuário não encontrado. ID: {UserId}", request.UserId);
            throw new TickestException($"Usuário com ID {request.UserId} não encontrado.");
        }

        #endregion

        #region Exclusão do Usuário

        await userRepository.DeleteAsync(userToDelete.Id, cancellationToken);
        logger.LogInformation("Usuário com ID {UserId} excluído com sucesso.", userToDelete.Id);

        #endregion

        return Result.Success(userToDelete.Id);
    }
}
