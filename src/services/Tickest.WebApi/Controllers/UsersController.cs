using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Users.Create;
using Tickest.Application.Users.GetCurrent;
using Tickest.Domain.Constants;
using WebApi.Authorization;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [HasPermission(SystemPermissions.ManageUsers)]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command, CancellationToken cancellationToken) =>
        Ok(await mediator.Send(command, cancellationToken));

    [HttpGet("me")]
    [HasPermission(SystemPermissions.AccessSystem)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken) =>
        Ok(await mediator.Send(new GetCurrentUserQuery(), cancellationToken));
}
