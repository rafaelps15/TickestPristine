using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Delete;

internal sealed class DeleteUserCommandHandler(
    IBaseRepository<User,Guid> userRepository,
    IAuthService authService,
    ILogger<DeleteUserCommandHandler> logger)
    : ICommandHandler<DeleteUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando solicitação de exclusão de usuário.");

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        var isAdmin = currentUser.UserRoles.Any(ur => 
            ur.Role?.Name == "Admin" || ur.Role?.Name == "AdminMaster");

        if (!isAdmin)
            throw new TickestException("Apenas administradores podem excluir usuários.");

        var userToDelete = await userRepository.GetByIdAsync(request.UserId);

        var userRoles = await userRepository.GetUserRolesAllAsync(request.UserId, cancellationToken);
        foreach (var userRole in userRoles)
        {
            await userRepository.DeleteUserRoleAsync(userRole, cancellationToken);  
        }

        logger.LogInformation("Associações de funções do usuário {UserId} removidas com sucesso.", userToDelete.Id);

        await userRepository.DeleteAsync(userToDelete.Id, cancellationToken);
        logger.LogInformation("Usuário {UserId} excluído com sucesso.", userToDelete.Id);

        return Result.Success(userToDelete.Id);
    }
}
