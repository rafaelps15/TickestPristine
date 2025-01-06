using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Sectors.Create;

internal sealed class CreateAreaCommandHandler(
    IPermissionProvider permissionProvider,
    IAuthService authService,
    IAreaRepository areaRepository,
    IUserRepository userRepository,
    ILogger<CreateAreaCommandHandler> logger)
    : ICommandHandler<CreateAreaCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAreaCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de uma nova área.");

        #region Verificação de Permissão do Usuário

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        const string requiredPermission = "CreateArea";
        await permissionProvider.ValidatePermissionAsync(currentUser, requiredPermission);

        logger.LogInformation("O usuário {UserId} tem permissão para criar uma área.", currentUser.Id);

        #endregion

        #region Validação do Usuário Responsável (Gestor da Área)

        // Verificando se o AreaManagerId foi fornecido e se o gerente existe
        if (command.AreaManagerId.HasValue)
        {
            var areaManager = await userRepository.GetByIdAsync(command.AreaManagerId.Value, cancellationToken);
            if (areaManager == null)
            {
                logger.LogError("Gestor da área com ID {UserId} não encontrado.", command.AreaManagerId);
                throw new TickestException("Gestor da área não encontrado.");
            }
        }

        #endregion

        #region Criação de uma Nova Área

        var area = new Area
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        if (command.AreaManagerId.HasValue)
        {
            area.AreaManagerId = command.AreaManagerId.Value;
        }

        logger.LogInformation("Área preparada para persistência: {AreaId}", area.Id);

        #endregion

        #region Persistência no Banco de Dados

        await areaRepository.AddAsync(area, cancellationToken);
        await areaRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Área {area.Name} criada com sucesso.");

        #endregion

        return Result.Success(area.Id); // Retorna o ID da nova área criada
    }
}
