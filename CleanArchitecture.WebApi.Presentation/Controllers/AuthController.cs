using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.Login;
using CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.RefreshToken;
using CleanArchitecture.WebApi.Presentation.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Presentation.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct = default)
    {
        var command = new LoginCommand(request.Email, request.Password);
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct = default)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        return Ok(await _mediator.Send(command, ct));
    }
}
