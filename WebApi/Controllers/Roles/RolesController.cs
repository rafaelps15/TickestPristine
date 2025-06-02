using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Features.Roles.GetAll;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApi.Controllers.Roles;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetAllRolesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

}