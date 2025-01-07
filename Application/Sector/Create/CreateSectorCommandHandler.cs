using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Sectors.Create;

internal sealed class CreateSectorCommandHandler(
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ISectorRepository sectorRepository,
    IDepartmentRepository departmentRepository,
    IUserRepository userRepository,
    ILogger<CreateSectorCommandHandler> logger)
    : ICommandHandler<CreateSectorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSectorCommand command, CancellationToken cancellationToken)
    {
        #region Verificação de Permissão do Usuário

        logger.LogInformation("Iniciando a criação de um novo setor.");

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        const string requiredPermission = "ManageSector";
        await permissionProvider.ValidatePermissionAsync(currentUser, requiredPermission);
        logger.LogInformation("O usuário {UserId} tem permissão para criar um setor.", currentUser.Id);

        #endregion

        var sector = new Sector
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            SectorManagerId = command.SectorManagerId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await sectorRepository.AddAsync(sector, cancellationToken);

        logger.LogInformation("Setor {SectorName} criado com sucesso com ID {SectorId}.", sector.Name, sector.Id);

        return Result.Success(sector.Id);

    }
}