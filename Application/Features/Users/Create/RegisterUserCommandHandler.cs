using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Create;

    internal sealed class RegisterUserCommandHandler(
     IUserRepository userRepository,
     IRoleRepository roleRepository,
     IPasswordHasher passwordHasher)
     : ICommandHandler<RegisterUserCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var hashedPassword = passwordHasher.Hash(command.Password);

            var user = new User
            {
                Name = command.Name,
                Email = command.Email,
                PasswordHash = hashedPassword
            };

            // Vincula roles existentes
            foreach (var roleName in command.Roles ?? new List<string>())
            {
                var role = await roleRepository.GetByNameAsync(roleName, cancellationToken);
                if (role == null)
                    throw new TickestException($"Função '{roleName}' não encontrada");

                user.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }

            await userRepository.AddAsync(user, cancellationToken);

            return user.Id;
    }
    }


