using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.DeleteUser;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.UpdateUser;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;
using CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetUserById;
using CleanArchitecture.WebApi.Presentation.Requests.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserResponse>>> GetAll(
        [FromQuery] int page = PaginationDefaults.DefaultPage,
        [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var query = new GetAllUsersQuery(page, pageSize, search);
        return Ok(await _mediator.Send(query, ct));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query, ct);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct = default)
    {
        var command = new CreateUserCommand(request.FirstName, request.LastName, request.Email, request.Password, request.RoleIds);
        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken ct = default)
    {
        var command = new UpdateUserCommand(id, request.FirstName, request.LastName, request.Email, request.RoleIds);
        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command, ct);

        return Ok();
    }
}
