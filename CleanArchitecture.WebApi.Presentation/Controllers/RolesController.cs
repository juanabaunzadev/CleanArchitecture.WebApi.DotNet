using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Roles;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Commands;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.DeleteRole;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.UpdateRole;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Queries.GetAllRoles;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Queries.GetRoleById;
using CleanArchitecture.WebApi.Presentation.Requests.Roles;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<RoleResponse>>> GetAll(
        [FromQuery] int page = PaginationDefaults.DefaultPage,
        [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var query = new GetAllRolesQuery(page, pageSize, search);
        return Ok(await _mediator.Send(query, ct));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var query = new GetRoleByIdQuery(id);
        var role = await _mediator.Send(query, ct);

        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken ct = default)
    {
        var command = new CreateRoleCommand(request.Name, request.PermissionIds);
        await _mediator.Send(command, ct);
        
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken ct = default)
    {
        var command = new UpdateRoleCommand(id, request.Name, request.PermissionIds);
        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var command = new DeleteRoleCommand(id);
        await _mediator.Send(command, ct);

        return Ok();
    }

}
