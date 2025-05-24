using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Features.Departments.Add;
using Tickest.Application.Features.Sectors.Create;
using Tickest.Application.Features.Users.Add;
using Tickest.Application.Sectors.Delete;
using Tickest.Application.Sectors.Get;
using Tickest.Application.Sectors.GetById;

namespace WebApi.Controllers.Sector;

public class SectorController : ControllerBase
{
    private readonly IMediator _mediator;

    public SectorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateSector([FromBody] CreateSectorCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // Listar Setores
    [HttpGet]
    public async Task<IActionResult> GetAllSectors()
    {
        var query = new GetAllSectorsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // Obter detalhes do setor por id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdSectorQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // Adicionar departamentos ao setor
    [AllowAnonymous]
    [HttpPost("add-department-to-sector")]
    public async Task<IActionResult> AddDepartments(AddDepartmentsToSectorCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // Adicionar usuário ao setor
    [HttpPost("add-user-to-sector")]
    public async Task<IActionResult> AddUserToSector([FromBody] AddUserToSectorCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // Deletar um setor
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSector(Guid id)
    {
        var command = new DeleteSectorCommand(id);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

}
