using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Add;

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
        logger.LogInformation("Iniciando a associação do usuário autenticado ao setor {SectorId}.", command.SectorId);

        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "AssignUserToSector");

        logger.LogInformation("Usuário {UserId} autorizado para criar.", currentUser.Id);

        #endregion

        #region Validação do Setor

        var sector = await sectorRepository.GetByIdAsync(command.SectorId, cancellationToken);

        if (sector == null)
        {
            throw new TickestException("Setor não encontrado.");
        }

        logger.LogInformation("Setor encontrado: {SectorId}.", command.SectorId);

        #endregion

        #region Associação do Usuário ao Setor

        // Atualiza a associação do usuário com o setor
        currentUser.SectorId = command.SectorId;
        await userRepository.UpdateAsync(currentUser, cancellationToken);
        logger.LogInformation("Usuário {UserId} associado ao setor {SectorId} com sucesso.", currentUser.Id, command.SectorId);

        #endregion

        #region Retorno

        return Result.Success(sector.Id);

        #endregion
    }
}
