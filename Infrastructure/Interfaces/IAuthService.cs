using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<TokenModel> AuthenticateAsync(Usuario usuario);
}
