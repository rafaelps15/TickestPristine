using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Features.Users.Create;

namespace WebApi.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator, IPermissionProvider permissionProvider)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "AdminMaster")]
        [HttpPost("create")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
