using MediatR;
using Tickest.Domain.Contracts.Models;
using Tickest.Infrastructure.Services.Authentication;

namespace Tickest.Application.Authentication.Commands.Tokens;

public class RenewTokenCommandHandler : IRequestHandler<RenewTokenCommand, TokenModel>
{
    private readonly ITokenService _tokenService;

    public RenewTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<TokenModel> Handle(RenewTokenCommand request, CancellationToken cancellationToken)
    {
        var newToken = await _tokenService.RenewTokenAsync(request.UserId); // Usando UserId
        return new TokenModel(newToken);
    }
}
