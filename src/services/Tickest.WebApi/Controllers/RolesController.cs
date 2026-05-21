using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Roles.GetAll;

namespace WebApi.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await mediator.Send(new GetAllRolesQuery(), cancellationToken));
}
