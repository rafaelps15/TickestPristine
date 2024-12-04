using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Users.GetById;

public class GetByIdUserQueryHandler : IQueryHandler<GetByIdUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdUserQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
    {
        // Obtém o usuário através do repositório
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        // Verifica se o usuário existe e está ativo
        if (user == null)
        {
            throw new TickestException("Usuário não encontrado.");
        }

        // Aplica o filtro de "Ativo" se necessário (caso o repositório retorne usuários filtrados)
        var query = _unitOfWork.Repository<User>().GetAll(); // Ou outro método de consulta relevante
        query = _unitOfWork.ApplyFilters(query);

        // Verifica se o usuário está ativo
        if (!query.Any(u => u.Id == user.Id))
        {
            throw new TickestException("Usuário encontrado, mas não está ativo.");
        }

        return new UserResponse(user.Id, user.Name);
    }
}
