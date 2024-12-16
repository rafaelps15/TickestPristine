using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Data;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler
    (IAuthService _authService, IApplicationDbContext _context, IPermissionProvider _permissionProvider, ISpecialtyRepository _specialtyRepository, IPermissionRepository permission)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        // Obtém o usuário autenticado atual
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        // Exemplo para permissão caso seja necessário limitar essa busca.
        // Verificando se o usuário tem permissão para visualizar outros usuários (por exemplo, "ViewUserDetails").
        // var hasPermission = await _permissionProvider.UserHasPermissionAsync(currentUser.Id, "ViewUserDetails");

        // Verifica se o usuário tem permissão para acessar os dados de outro usuário
        if (query.UserId != currentUser.Id)
        {
            throw new TickestException("Você não tem permissão para acessar esses dados.");
        }

        // Busca o usuário no banco de dados
        var user = await _context.Users
            .Where(u => u.Id == query.UserId) 
            .Include(u => u.UserSpecialties) 
            .Include(u => u.Permissions)    
            .Select(u => new UserResponse(
                u.Id,
                u.Name,
                u.Email,
                u.UserSpecialties.Select(us => us.Specialty.Name).ToList(),
                u.Permissions.Select(p => p.Description).ToList()
))

            .FirstOrDefaultAsync(cancellationToken);

        // Se o usuário não for encontrado, lança uma exceção
        if (user == null)
        {
            throw new TickestException($"Usuário com ID {query.UserId} não encontrado.");
        }

        // Retorna o usuário encontrado
        return user;
    }
}
