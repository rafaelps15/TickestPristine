using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Sectors.Create;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Sectors.Get;

internal sealed class CreateSectorCommandHandler(
    ISectorRepository sectorRepository,
    IUserRepository userRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<CreateSectorCommandHandler> logger)
    : ICommandHandler<CreateSectorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSectorCommand command, CancellationToken cancellationToken)
    {
        #region Validação de Permissões

        //var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        //await permissionProvider.ValidatePermissionAsync(currentUser, "CreateSector");

        #endregion

        #region Criação do Setor

        var sector = new Sector
        {
            Name = command.Name,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow
        };

        if (command.SectorManagerId.HasValue)
        {
            var currentUserResponsible = await userRepository.GetByIdAsync(command.SectorManagerId.Value);
            if (currentUserResponsible is null)
            {
                throw new TickestException("Usuário responsável não encontrado.");
            }

            await permissionProvider.ValidatePermissionAsync(currentUserResponsible, "CanUserBeResponsible");

            sector.SectorManager = currentUserResponsible;
        }

        #endregion

        #region Salvamento no Repositório

        await sectorRepository.AddAsync(sector, cancellationToken);
        logger.LogInformation("Setor criado com sucesso: {SectorId}", sector.Id);

        #endregion

        return Result.Success(sector.Id);
    }
}
