using Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tickest.Application.Tickets.Create;
using Tickest.Application.Tickets.Update;
using Tickest.Infrastructure.Authentication;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TicketController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<TicketController> _logger;
    private readonly IMediator _mediator;
    private readonly JwtSettings _jwtSettings;

    public TicketController(
        IAuthorizationService authorizationService,
        ILogger<TicketController> logger,
        IOptions<JwtSettings> jwtSettings,
        IMediator mediator)
    {
        (_authorizationService, _logger, _jwtSettings, _mediator) =
               (authorizationService, logger, jwtSettings.Value, mediator);
    }

    [HttpPost]
    [HasPermission("create")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand command) =>
        Ok(await _mediator.Send(command));

    [HttpGet("{id}")]
    [HasPermission("ViewTicket")]
    public async Task<IActionResult> GetTicketById(Guid id)
    {
        return Ok();
    }

    [HttpPut("{id}")]
    [HasPermission("ManageTickets")]
    public async Task<IActionResult> UpdateTicketStatus(Guid id, [FromBody] UpdateTicketCommand command) =>
        Ok(await _mediator.Send(command));
}
