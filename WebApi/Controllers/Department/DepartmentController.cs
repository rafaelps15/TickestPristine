using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Features.Areas.Add;
using Tickest.Application.Features.Departments.Create;
using Tickest.Application.Features.Departments.Get;

namespace WebApi.Controllers.Department;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    //GET para listar departamentos
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartment(Guid id)
    {
        var result = await _mediator.Send(new GetDepartmentsQuery(id));
        return Ok(result);
    }


    // testar *******************************************************
    // POST para adicionar áreas a um departamento
    [HttpPost("{departmentId:guid}/areas")]
    public async Task<IActionResult> AddAreasToDepartment([FromRoute] Guid departmentId , [FromBody] AddAreasToDepartmentsCommand command)
    {
        command = command with { DepartmentId = departmentId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}
