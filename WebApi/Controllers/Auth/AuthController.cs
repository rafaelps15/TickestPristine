using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Features.Users.Auth;

namespace WebApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserCommand command)
        => Ok(await _mediator.Send(command));


    // Esse método será usado junto ao login do usuario para que gere outro token (RefreshToken)
    // para que o usuario nao precise logar de novo. 
    [HttpPost("renew")]
    public async Task<IActionResult> Renew([FromBody] RenewTokenCommand command)
        => Ok(await _mediator.Send(command));
}
