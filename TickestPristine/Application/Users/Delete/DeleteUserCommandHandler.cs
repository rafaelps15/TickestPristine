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

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado. Requisição de exclusão não permitida.");
            throw new TickestException("Usuário não autenticado. Operação de exclusão falhou.");
        }

        await permissionProvider.ValidatePermissionAsync(currentUser.Id, "DeleteUser");

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

        await userRepository.DeleteByIdAsync(userToDelete.Id, cancellationToken);
        logger.LogInformation("Usuário com ID {UserId} excluído com sucesso.", userToDelete.Id);

        #endregion

        return Result<Guid>.Success(userToDelete.Id);
    }
}
