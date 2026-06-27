using Microsoft.EntityFrameworkCore;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler(
    IUserContext userContext,
    IApplicationDbContext context)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            throw new TickestException("Você não tem permissão para acessar esses dados.");
        }

        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Include(u => u.UserSpecialties)
                .ThenInclude(us => us.Specialty)
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            throw new TickestException($"Usuário com ID {query.UserId} não encontrado.");
        }

        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.RoleId,
            user.Role.Name,
            user.UserSpecialties.Select(us => us.Specialty.Name).ToList(),
            user.Permissions.Select(p => p.Description).ToList());
    }
}
