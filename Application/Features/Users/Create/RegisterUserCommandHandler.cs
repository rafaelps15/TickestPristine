using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Create;

internal sealed class RegisterUserCommandHandler(
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();

            #region Validação de Usuário

            if (await userRepository.GetUserByEmailAsync(command.Email, cancellationToken) is not null)
                throw new TickestException("O e-mail fornecido já está em uso.");

            #endregion

            #region Obter e Validar Papel

            var role = await roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role == null)
                throw new TickestException("O papel fornecido não é válido.");

            #endregion

            #region Criação do Usuário

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
                IsActive = true,
                UserRoles = new List<UserRole>()
            };

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            user.UserRoles.Add(userRole);

            #endregion

            #region Persistência

            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            await unitOfWork.CommitTransactionAsync();

            #endregion

            return Result.Success(user.Id);
        }

        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
