using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Users.Create;
using Tickest.Domain.Constants;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Endpoint to create a new user (Somente Admin e AdminMaster podem criar usuários)
    [HttpPost]
    [HasPermission(SystemPermissions.ManageUsers)]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command) =>
        Ok(await _mediator.Send(command));
}
