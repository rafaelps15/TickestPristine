using Tickest.Application.DTOs;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    TokenResponse Create(User user);
}
