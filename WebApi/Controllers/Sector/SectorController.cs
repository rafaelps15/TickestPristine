using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Departments.Add;
using Tickest.Application.Departments.Get;
using Tickest.Application.Sectors.Create;
using Tickest.Application.Sectors.Get;
using Tickest.Application.Users.Add;

namespace WebApi.Controllers.Sector
{
    public class SectorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SectorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSector([FromBody] CreateSectorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //GET Listar Setores
        [HttpGet]
        public async Task<IActionResult> GetSectorDetails(Guid id)
        {
            var result = await _mediator.Send(new GetSectorsDetailsQuery(id));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetSectorsDetailsQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("add-departmenrts")]
        public async Task<IActionResult> AddDepartments(AddDepartmentsToSectorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUserToSector([FromBody] AddUserToSectorCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
