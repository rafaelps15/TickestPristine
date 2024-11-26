using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tickest.Application.Users.Get
{
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

            // 1. Obter as UserRoles associadas ao usuário a partir do repositório
            var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserId == userId, cancellationToken);

            // Verificar se foram encontradas UserRoles
            if (userRoles == null || !userRoles.Any())
                throw new TickestException("Nenhuma role encontrada para este usuário.");

            // 2. Obter o usuário
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
                ?? throw new TickestException($"Usuário com Id {userId} não encontrado.");

            // 3. Obter as roles associadas às UserRoles
            var roles = userRoles.Select(ur => ur.Role).ToList();

            // 4. Criar o UserResponse com as roles encontradas
            var userResponse = new UserResponse(user.Id, user.Name, roles.Select(role => role.Name).ToList());

            // Retornar o UserResponse
            return userResponse;
        }
    }
}
