using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;
using CleanArchitecture.WebApi.Presentation.Requests.Users;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(request.FirstName, request.LastName, request.Email, request.Password);
        await _mediator.Send(command);
        
        return Ok();
    }
}
