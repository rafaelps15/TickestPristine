using Microsoft.EntityFrameworkCore;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Persistence.Data;

namespace Tickest.Application.Users.GetById;

internal sealed class GetByIdUserQueryHandler(
    IAuthService _authService,
    TickestContext _context,
    IPermissionProvider _permissionProvider 

) : IQueryHandler<GetByIdUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetByIdUserQuery query, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        //Exemplo para permissão caso seja necessário limitar essa busca . 
        // Verificando se o usuário tem permissão para visualizar outros usuários (por exemplo, "ViewUserDetails")
        //var hasPermission = await _permissionProvider.UserHasPermissionAsync(currentUser.Id, "ViewUserDetails");
        if (query.UserId != currentUser.Id)
        {
            throw new TickestException("Você não tem permissão para acessar esses dados.");
        }

        var user = await _context.Users
            .Where(u => u.Id == query.UserId)
            .Select(u => new UserResponse(
                u.Id,
                u.Name,
                u.Email,
                u.UserSpecialties.Select(us => us.Specialty.Name).ToList(),
                u.Permissions.Select(p => p.Name).ToList()
            ))
            .SingleOrDefaultAsync(cancellationToken);

        return user is null
            ? throw new TickestException($"Usuário com ID {query.UserId} não encontrado.")
            : Result.Success(user);
    }
}
