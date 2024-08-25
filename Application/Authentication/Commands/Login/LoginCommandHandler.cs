using MediatR;
using Tickest.Domain.Contracts.Models;
using Tickest.Infrastructure.Services.Auth;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenModel>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        this._authService = authService;
    }

    public Task<TokenModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
} 
