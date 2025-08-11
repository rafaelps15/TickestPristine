using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Features.Users.Login;

namespace WebApi.Controllers.Auth
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
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command) =>
            Ok(await _mediator.Send(command));


        // Esse método será usado junto ao login do usuario para que gere outro token (RefreshToken)
        // para que o usuario nao precise logar de novo. -> ainda preciso implentar 
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
}
