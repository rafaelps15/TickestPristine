using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Users.Get;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository) =>
        _userRepository = userRepository;

    public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)  // Verifica se o ID foi fornecido
        {
            var user = await _userRepository.GetByIdAsync(request.Id.Value, cancellationToken);
            if (user == null) throw new TickestException("Usuário não encontrado.");

            var roles = user.UserRoles.Select(ur => ur.Role).ToList();

            return new UserResponse(user.Id, user.Name, roles, user.Email);
        }

        if (!string.IsNullOrEmpty(request.Name))  // Verifica se o nome foi fornecido
        {
            var user = await _userRepository.GetByNameAsync(request.Name);
            if (user == null) throw new TickestException("Usuário não encontrado.");

            var roles = user.UserRoles.Select(ur => ur.Role).ToList();

            return new UserResponse(user.Id, user.Name, roles, user.Email);
        }

        throw new TickestException("Nenhum critério de busca válido fornecido.");
    }
}
