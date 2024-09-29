using MediatR;
using Tickest.Domain.Contracts.Models;

namespace Tickest.Application.Authentication.Commands.Tokens;

public class RenewTokenCommand : IRequest<TokenModel>
{
    public string UserId { get; }

    public RenewTokenCommand(string userId)
    {
        UserId = userId;
    }
}
