using Microsoft.EntityFrameworkCore;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Users.GetCurrent;

internal sealed class GetCurrentUserQueryHandler(
    IUserContext userContext,
    IApplicationDbContext context)
    : IQueryHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    public async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Include(u => u.UserSpecialties)
                .ThenInclude(us => us.Specialty)
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == userContext.UserId, cancellationToken);

        if (user is null)
        {
            throw new TickestException("Usuário não encontrado.");
        }

        return new GetCurrentUserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.RoleId,
            user.UserSpecialties.Select(us => us.Specialty.Name).ToList(),
            user.Permissions.Select(p => p.Description).ToList());
    }
}
