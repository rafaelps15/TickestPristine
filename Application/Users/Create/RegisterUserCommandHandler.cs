using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IPermissionProvider permissionProvider,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        //a pessoa logada pode criar esse usuário?

        #region Validação do Usuário Existente

        var existingUser = await userRepository.GetUserByEmailAsync(command.Email, cancellationToken);

        if (existingUser != null)
        {
            logger.LogError("O e-mail {Email} já está em uso.", command.Email);
            throw new TickestException("O e-mail fornecido já está em uso.");
        }

        #endregion

        #region Obter Papéis Disponíveis

        // Verifica se o papel fornecido existe entre os papéis disponíveis
        var role = await roleRepository.GetByIdAsync(command.RoleId);
        if (role == null)
        {
            logger.LogError("O papel fornecido {Role} não existe.", command.RoleId);
            throw new TickestException("O papel fornecido não é válido.");
        }

        #endregion

        #region Criação do Novo Usuário

        var salt = EncryptionHelper.CreateSaltKey(command.Password.Length);
        var password = EncryptionHelper.CreatePasswordHashWithSalt(command.Password, salt);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Name = command.Name,
            PasswordHash = password,
            Salt = salt,
            CreatedAt = DateTime.UtcNow,
            Role = role, // Define o papel do novo usuário
            IsActive = true
        };

        // Obtém as permissões com base no papel do usuário OBS: Inicialmente para testes, depois será inserido no banco de dados.
        var permissions = permissionProvider.GetPermissionsForRole(role.Name);

        // Adicionar as permissões ao usuário (se necessário, caso o modelo de usuário suporte e necessite)
        logger.LogInformation("Usuário preparado para persistência: {UserId}.", user.Id);

        #endregion

        #region Persistência no Banco de Dados

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync();

        logger.LogInformation($"Usuário {user.Name} com o papel {user.Role} criado com sucesso.");

        #endregion

        return Result.Success(user.Id);
    }
}
