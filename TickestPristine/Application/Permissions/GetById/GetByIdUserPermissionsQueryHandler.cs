//using MediatR;
//using Tickest.Application.Abstractions.Messaging;
//using Tickest.Domain.Common;
//using Tickest.Domain.Entities.Permissions;
//using Tickest.Domain.Interfaces.Repositories;


//namespace Tickest.Application.Permissions.GetById;

//public class GetByIdUserPermissionsQueryHandler : IQueryHandler<GetByIdUserPermissionsQuery, IEnumerable<Permission>>
//{
//    private readonly IUserRepository _userRepository;

//    public GetByIdUserPermissionsQueryHandler(IUserRepository userRepository) =>

//        _userRepository = userRepository;


//    public async Task<IEnumerable<Permission>> Handle(GetByIdUserPermissionsQuery query, CancellationToken cancellationToken)
//    {
//        var user = await _userRepository.GetByIdAsync(query.UserId);

//        if (user == null)
//        {
//            return Enumerable.Empty<Permission>();
//        }

//        // Retorna as permissões associadas ao usuário
//        return user.Permissions ?? Enumerable.Empty<Permission>();
//    }

//}
