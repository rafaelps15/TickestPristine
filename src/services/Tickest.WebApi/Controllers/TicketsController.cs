using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Tickets.Create;
using Tickest.Application.Tickets.Update;
using Tickest.Domain.Constants;
using WebApi.Authorization;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/tickets")]
public class TicketsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [HasPermission(SystemPermissions.CreateTicket)]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand command, CancellationToken cancellationToken) =>
        Ok(await mediator.Send(command, cancellationToken));

    [HttpPut("{id:guid}")]
    [HasPermission(SystemPermissions.AccessSystem)]
    public async Task<IActionResult> UpdateTicketStatus(Guid id, [FromBody] UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        var request = command with { TicketId = id };

        return Ok(await mediator.Send(request, cancellationToken));
    }
}
