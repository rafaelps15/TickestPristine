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
    IUserRepository userRepository,
    ILogger<CreateSectorCommandHandler> logger)
    : ICommandHandler<CreateSectorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSectorCommand command, CancellationToken cancellationToken)
    {
        #region Verificação de Permissão do Usuário

        logger.LogInformation("Iniciando a criação de um novo setor.");

        // Obtém o usuário autenticado via token
        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        var currentUserId = currentUser?.Id;

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        // Valida a permissão do usuário para criar setor
        const string requiredPermission = "ManageSector";
        await permissionProvider.ValidatePermissionAsync(currentUser, requiredPermission);

        logger.LogInformation("O usuário {UserId} tem permissão para criar um setor.", currentUser.Id);

        #endregion

        #region Validação do Gestor do Setor

        // Validação do gestor do setor (se fornecido)
        if (command.SectorManagerId.HasValue)
        {
            var sectorManager = await userRepository.GetByIdAsync(command.SectorManagerId.Value, cancellationToken);
            if (sectorManager == null)
            {
                logger.LogError("Gestor do setor com id {userId} não encontrado.", command.SectorManagerId);
                throw new Exception("Gestor do setor não encontrado.");
            }
        }

        #endregion

        #region Criação do Setor

        // Criação do setor
        var sector = new Sector
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            SectorManagerId = command.SectorManagerId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Departments = new List<Department>() // Inicializa a lista de departamentos
        };

        #endregion

        #region Persistência do Setor

        // Persiste o setor no banco de dados
        await sectorRepository.AddAsync(sector, cancellationToken);
        await sectorRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation($"Setor {sector.Name} criado com sucesso.");

        #endregion

        return Result.Success(sector.Id);
    }
}
