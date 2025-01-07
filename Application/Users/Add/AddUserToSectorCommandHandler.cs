using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Add;

internal sealed class AddUserToSectorCommandHandler(
    IAuthService authService,
    ISectorRepository sectorRepository,
    IUserRepository userRepository,
    IPermissionProvider permissionProvider,
    ILogger<AddUserToSectorCommandHandler> logger)
    : ICommandHandler<AddUserToSectorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddUserToSectorCommand command, CancellationToken cancellationToken)
    {
        #region Log de Início

        logger.LogInformation("Iniciando a associação do usuário autenticado ao setor {SectorId}.", command.SectorId);

        #endregion

        #region Recuperação do Usuário Autenticado

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        if (currentUser == null)
        {
            logger.LogError("Usuário autenticado não encontrado.");
            throw new TickestException("Usuário não autenticado.");
        }

        logger.LogInformation("Usuário autenticado: {UserId}.", currentUser.Id);

        #endregion

        #region Verificação de Permissão

        const string requiredPermission = "AdminMaster, AdminGeral";
        await permissionProvider.ValidatePermissionAsync(currentUser, requiredPermission);
        logger.LogInformation("Usuário {UserId} autorizado para associar ao setor.", currentUser.Id);

        #endregion

        #region Validação do Setor

        var sector = await sectorRepository.GetByIdAsync(command.SectorId, cancellationToken);
        if (sector == null)
        {
            logger.LogError("Setor com ID {SectorId} não encontrado.", command.SectorId);
            throw new TickestException("Setor não encontrado.");
        }

        logger.LogInformation("Setor encontrado: {SectorId}.", command.SectorId);

        #endregion

        #region Associação do Usuário ao Setor

        var user = currentUser; 
        user.SectorId = command.SectorId;

        await userRepository.UpdateAsync(user, cancellationToken);
        logger.LogInformation("Usuário {UserId} associado ao setor {SectorId} com sucesso.", user.Id, command.SectorId);

        #endregion

        #region Retorno

        return Result.Success(sector.Id);

        #endregion
    }
}
