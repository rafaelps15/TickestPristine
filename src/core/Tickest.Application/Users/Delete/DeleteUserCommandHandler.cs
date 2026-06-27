using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Users.Delete;

internal sealed class DeleteUserCommandHandler(
    IBaseRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext,
    IPermissionProvider permissionProvider,
    ILogger<DeleteUserCommandHandler> logger)
    : ICommandHandler<DeleteUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando solicitação de exclusão de usuário.");

        await permissionProvider.ValidatePermissionAsync(userContext.UserId, SystemPermissions.DeleteUser);

        var userToDelete = await userRepository.GetByIdAsync(request.UserId);

        if (userToDelete == null)
        {
            logger.LogError("Usuário não encontrado. ID: {UserId}", request.UserId);
            throw new TickestException($"Usuário com ID {request.UserId} não encontrado.");
        }

        await userRepository.DeleteByIdAsync(userToDelete.Id, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Usuário com ID {UserId} excluído com sucesso.", userToDelete.Id);

        return Result<Guid>.Success((Guid)userToDelete.Id);
    }
}
