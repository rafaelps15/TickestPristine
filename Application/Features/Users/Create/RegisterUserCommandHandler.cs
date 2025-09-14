using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Create;

internal sealed class RegisterUserCommandHandler(
 IUserRepository _userRepository,
 IRoleRepository _roleRepository,
 IPasswordHasher _passwordHasher)
 : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        // Gerar salt único para o usuário
        var salt = Guid.NewGuid().ToString();

        // Hash da senha com salt
        var hashedPassword = _passwordHasher.Hash(command.Password + salt);

        var user = new User
        {
            Name = command.Name,
            Email = command.Email,
            PasswordHash = hashedPassword,
            Salt = salt, // não permitir NULL
            UserRoles = new List<UserRole>()
        };

        // Vincula roles existentes
        foreach (var roleName in command.Roles ?? new List<string>())
        {
            var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);
            if (role == null)
                throw new TickestException($"Função '{roleName}' não encontrada");

            // Associação direta para evitar conflito de FK
            user.UserRoles.Add(new UserRole
            {
                RoleId = role.Id,
                User = user
            });
        }

        await _userRepository.AddAsync(user, cancellationToken);

        return Result.Success(user.Id); 
    }
}
