using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Users.Create;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator=mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command) =>
            Ok(await _mediator.Send(command));
    }
}
