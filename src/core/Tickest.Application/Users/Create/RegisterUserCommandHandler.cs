using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Abstractions.ReadServices;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IUserRegistrationReadService readService,
    IPasswordHasher passwordHasher,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        if (await readService.EmailExistsAsync(command.Email, cancellationToken))
        {
            logger.LogError("O e-mail {Email} já está em uso.", command.Email);
            throw new TickestException("O e-mail fornecido já está em uso.");
        }

        if (await readService.EmployeeCodeExistsAsync(command.EmployeeCode, cancellationToken))
        {
            logger.LogError("O código de funcionário {EmployeeCode} já está em uso.", command.EmployeeCode);
            throw new TickestException("O código de funcionário fornecido já está em uso.");
        }

        var specialtyIds = command.SpecialtyIds?.Distinct().Select(id => (EntityId)id).ToList() ?? [];

        var roleId = await readService.GetActiveRoleIdByNameAsync(SystemRoles.Collaborator, cancellationToken);

        if (roleId is null)
        {
            throw new TickestException("Função padrão de colaborador não encontrada ou inativa.");
        }

        if (command.SectorId.HasValue && !await readService.ActiveSectorExistsAsync(command.SectorId.Value, cancellationToken))
        {
            throw new TickestException("Setor inválido.");
        }

        if (specialtyIds.Count > 0 && !await readService.ActiveSpecialtiesExistAsync(specialtyIds, cancellationToken))
        {
            throw new TickestException("Uma ou mais especialidades são inválidas.");
        }

        var user = User.Create(
            command.Name,
            command.EmployeeCode,
            command.Email,
            passwordHasher.Hash(command.Password),
            roleId,
            command.SectorId);

        foreach (var specialtyId in specialtyIds)
        {
            user.AddSpecialty(specialtyId);
        }

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Novo usuário criado com ID {UserId}.", user.Id);

        return Result.Success((Guid)user.Id);
    }
}
