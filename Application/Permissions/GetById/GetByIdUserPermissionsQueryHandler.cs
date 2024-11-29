using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;


namespace Tickest.Application.Permissions.GetById;

public class GetByIdUserPermissionsQueryHandler : IQueryHandler<GetByIdUserPermissionsQuery, IEnumerable<Permission>>
{
    private readonly IUserRepository _userRepository;

    public GetByIdUserPermissionsQueryHandler(IUserRepository userRepository) =>

        _userRepository = userRepository;


    public async Task<IEnumerable<Permission>> Handle(GetByIdUserPermissionsQuery query, CancellationToken cancellationToken)
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
