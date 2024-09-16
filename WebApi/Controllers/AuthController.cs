using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Authentication.Commands.Login;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
