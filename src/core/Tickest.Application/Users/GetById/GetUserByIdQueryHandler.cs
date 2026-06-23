using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler(IUserContext usercontext, IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = usercontext.UserId;

        if (query.UserId != currentUserId)
        {
            throw new TickestException("Voce nao tem permissao para acessar esses dados.");
        }

        var user = await userRepository.GetWithPermissionsAsync(query.UserId, cancellationToken);

        if (user is null)
        {
            throw new TickestException($"Usuario com ID {query.UserId} nao encontrado.");
        }

        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.RoleId,
            user.Role.Name,
            user.UserSpecialties.Select(userSpecialty => userSpecialty.Specialty.Name).ToList(),
            user.Permissions.Select(permission => permission.Description).ToList());
    }
}
