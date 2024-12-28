using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{

    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        #region Validação do Usuário Existente

        var existingUser = await userRepository.GetUserByEmailAsync(command.Email, cancellationToken);

        if (existingUser != null)
        {
            logger.LogError("O e-mail {Email} já está em uso.", command.Email);
            throw new TickestException("O e-mail fornecido já está em uso.");
        }

        #endregion

        #region Criação do Novo Usuário

        var (passwordHash, salt) = passwordHasher.HashWithSalt(command.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Name = command.Name,
            //PasswordHash = passwordHasher.HashWithSalt(command.Password),
            PasswordHash = passwordHash,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            Role = command.Role  // Define o papel do novo usuário
        };

        logger.LogInformation("Usuário preparado para persistência: {UserId}.", user.Id);

        #endregion

        #region Persistência no Banco de Dados

        await userRepository.AddAsync(user, cancellationToken);

        await userRepository.SaveChangesAsync();

        logger.LogInformation($"Usuário {user.Name} com o papel {user.Role} criado com sucesso.");


        #endregion

        #region Log de Auditoria
        //Caso seja necessario criar auditoria irei implementar.

        //logger.LogInformation("Usuário {UserId} criado por {CreatorId} em {CreationTime}.", user.Id, currentUser.Id, DateTime.UtcNow);

        #endregion

        return Result.Success(user.Id);
    }
}
