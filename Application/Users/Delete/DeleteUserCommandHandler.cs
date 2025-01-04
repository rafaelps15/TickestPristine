using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Domain.Common;
using MediatR;

namespace Tickest.Application.Users.Delete;

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
        var currenteUserId = currentUser.Id;

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não encontrado.");
        }

        // Verificando se o usuário tem permissão 
        var hasPermission = await permissionProvider.UserHasPermissionAsync(currentUser, "DeleteUser");
        if (!hasPermission)
        {
            logger.LogWarning("Usuário {userId} não tem permisão para deletar o ticket.", currenteUserId);
            throw new Exception("Usuário não tem permisisão para deletar");
        }

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

        return Result<Guid>.Success(userToDelete.Id);
    }
}
