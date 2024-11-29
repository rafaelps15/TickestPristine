using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.GetById;

public class GetByIdUserQueryHandler : IQueryHandler<GetByIdUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetByIdUserQueryHandler(IUserRepository userRepository) =>
        _userRepository = userRepository;


    public async Task<UserResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id,cancellationToken);

        return new UserResponse(user.Id, user.Name);
    }
}
