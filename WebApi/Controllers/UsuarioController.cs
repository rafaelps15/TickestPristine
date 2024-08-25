using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Users.CriarUsuario;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator=mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioCommand command)
        {


            await _mediator.Send(command);

            return Ok();
        }
    }
}
