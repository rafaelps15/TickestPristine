using Microsoft.AspNetCore.Http;
using Tickest.Application.Abstractions.Authentication;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new TickestException("Não foi possível obter o ID do usuário a partir do contexto HTTP.");
}
