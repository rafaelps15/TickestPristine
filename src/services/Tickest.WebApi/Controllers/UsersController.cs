using Tickest.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Users.Create;
using Tickest.Application.Users.GetById;
using Tickest.Application.Users.GetCurrent;
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

    // Cria um novo usuário. Somente Admin e AdminMaster podem criar usuários.
    [HttpPost]
    [HasPermission(SystemPermissions.ManageUsers)]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command) =>
        Ok(await _mediator.Send(command));

    [HttpGet("me")]
    [HasPermission(SystemPermissions.AccessSystem)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetCurrentUserQuery(), cancellationToken));
}
