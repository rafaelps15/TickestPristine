using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Users.GetById;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;


namespace Tickest.Application.Users.GetPermissions;

public class GetUserByIdPermissionsQueryHandler : IQueryHandler<GetUserByIdPermissionsQuery, IEnumerable<Permission>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdPermissionsQueryHandler(IUserRepository userRepository) =>
    
        _userRepository = userRepository;
    

    public async Task<IEnumerable<Permission>> Handle(GetUserByIdPermissionsQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);

        if (user == null)
        {
            return Enumerable.Empty<Permission>();
        }

        // Retorna as permissões associadas ao usuário
        return user.Permissions ?? Enumerable.Empty<Permission>();
    }
}
