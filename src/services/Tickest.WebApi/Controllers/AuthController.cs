using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Users.Create;
using Tickest.Application.Users.Login;

namespace WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Login do usuário. Retorna o token JWT.
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command) =>
        Ok(await _mediator.Send(command));


    // Registra uma nova conta pelo próprio usuário.
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command) =>
        Ok(await _mediator.Send(command));

    [HttpPost("renew")]
    [Authorize]
    public async Task<IActionResult> Renew()
    {
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //var command = new RenewTokenCommand(userId);
        //var newTokenModel = await _mediator.Send(command);
        //return Ok(newTokenModel);

        return Ok();
    }
}
