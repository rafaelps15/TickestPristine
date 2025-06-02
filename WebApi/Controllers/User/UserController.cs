using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Features.Users.Create;

namespace WebApi.Controllers.User
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionProvider _permissionProvider;

        public UserController(IMediator mediator, IPermissionProvider permissionProvider)
        {
            _mediator = mediator;
            _permissionProvider = permissionProvider;
        }

        //[Authorize(Roles = "AdminMaster")]
        // Endpoint para criar um novo usuário
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
