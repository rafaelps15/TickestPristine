using MediatR;
using Tickest.Domain.Contracts.Responses;

namespace Tickest.Application.Authentication.Commands.Tokens;

public class RenewTokenCommand : IRequest<TokenResponse>
{
    public string UserId { get; }

    public RenewTokenCommand(string userId)
    {
        UserId = userId;
    }
}
