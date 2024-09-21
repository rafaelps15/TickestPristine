using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Authentication.Commands.Login;
using Tickest.Domain.Entities;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IMediator _mediator;

	public AuthController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public async Task<IActionResult> Authenticate([FromBody] LoginCommand command)
	{
		var result = await _mediator.Send(command);

		return Ok(result);
	}
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] Usuario usuario)
	{
		var tokenModel = await _mediator.AuthenticateAsync(usuario);
		if (tokenModel == null)
		{
			return Unauthorized();

		}
		return Ok(tokenModel);

	}
	[HttpPost("renew")]
	[Authorize]
	public async Task<IActionResult> Renew()
	{
		var usuario = 
		var tokenModel = a
	}
}
