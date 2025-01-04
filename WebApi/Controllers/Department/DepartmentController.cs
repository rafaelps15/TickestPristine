using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickest.Application.Departments.Create;
using Tickest.Application.Departments.Get;

namespace WebApi.Controllers.Department;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    //GET para listar departamentos
    [HttpGet]
    public async Task<IActionResult> GetDepartment(Guid id)
    {
        var department = await _mediator.Send(new GetDepartmentsQuery(id));
        return department != null ? Ok(department) : NotFound();
    }

}
