using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Sectors.Create;

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

        // Validações serão relizadas na controller através da role. 
        // Criar o Setor e vincular os departamentos que estarão vinculados a ele.
        // Pensar sobre (vincular o setor a departamento), pois um Setor tem varios departamentos e assim por diante.
        //Setor - tem departamentos
        //Departamentos tem áreas
        //Áreas tem especialidade
        //Quero que o usuario AdminMaster e AdminGeral possam criar novos setores

     
        #endregion

        #region Criação do Setor

        var sector = new Sector
        {
            Name = command.Name,
            Description = command.Description,
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
