using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Permissions;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.CreatePermission;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.DeletePermission;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.UpdatePermission;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetAllPermissions;
using CleanArchitecture.WebApi.Application.UseCases.Permissions.Queries.GetPermissionById;
using CleanArchitecture.WebApi.Presentation.Requests.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<PermissionResponse>>> GetAll(
        [FromQuery] int page = PaginationDefaults.DefaultPage,
        [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var query = new GetAllPermissionsQuery(page, pageSize, search);
        return Ok(await _mediator.Send(query, ct));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PermissionResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var query = new GetPermissionByIdQuery(id);
        var permission = await _mediator.Send(query, ct);

        return Ok(permission);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken ct = default)
    {
        var command = new CreatePermissionCommand(request.Name, request.Description);
        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionRequest request, CancellationToken ct = default)
    {
        var command = new UpdatePermissionCommand(id, request.Name, request.Description);
        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var command = new DeletePermissionCommand(id);
        await _mediator.Send(command, ct);

        return NoContent();
    }
}
