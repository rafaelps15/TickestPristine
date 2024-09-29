using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tickest.Application.Authentication.Commands;
using Tickest.Application.Authentication.Commands.Login;
using Tickest.Application.Authentication.Commands.Tokens;

namespace WebApi.Controllers
{
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
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var tokenModel = await _mediator.Send(command);
            return Ok(tokenModel);
        }

        [HttpPost("renew")]
        [Authorize]
        public async Task<IActionResult> Renew()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = new RenewTokenCommand(userId);
            var newTokenModel = await _mediator.Send(command);
            return Ok(newTokenModel);
        }
    }
}
