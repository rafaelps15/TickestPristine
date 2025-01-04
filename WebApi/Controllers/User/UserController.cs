using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Users.Create;

namespace WebApi.Controllers.User
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionProvider _permissionProvider;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize(Roles = "AdminMaster")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command) =>
            Ok(await _mediator.Send(command));

        // Endpoint para obter os papéis disponíveis e retornar para o front
        [AllowAnonymous]
        [HttpGet("roles")]
        public IActionResult GetAvailableRoles()
        {
            var roles = _permissionProvider.GetAvailableRoles();
            return Ok(roles);
        }
    }
}
