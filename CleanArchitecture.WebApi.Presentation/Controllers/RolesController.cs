using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.UseCases.Roles.Commands;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken ct = default)
    {
        var command = new CreateRoleCommand(request.Name);
        await _mediator.Send(command, ct);
        
        return Ok();
    }
}
