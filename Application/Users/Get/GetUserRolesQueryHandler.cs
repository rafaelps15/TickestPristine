using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;

namespace Tickest.Application.Users.Get;

public sealed class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, UserResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserRolesQueryHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task<UserResponse> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        // Verificar se o Id do usuário foi fornecido
        if (!request.Id.HasValue)
            throw new TickestException("O Id do usuário é obrigatório.");

        var userId = request.Id.Value;

        // 1. Obter as UserRoles associadas ao usuário
        var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserId == userId, cancellationToken)
            ?? throw new TickestException("Nenhuma role encontrada para este usuário.");

        // 2. Obter o usuário
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
            ?? throw new TickestException($"Usuário com Id {userId} não encontrado.");

        // 3. Criar o UserResponse com as roles associadas ao usuário
        // Utilizando o UserResponseFactory para simplificar a criação
        return UserResponseFactory.CreateFromUserRoles(user.Id, user.Name, "Roles carregadas com sucesso.", userRoles);
    }
}
