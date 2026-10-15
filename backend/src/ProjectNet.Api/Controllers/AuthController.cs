using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectNet.Application.Commands.Login;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new LoginCommand(request.Username, request.Password), cancellationToken));
}

public sealed record LoginRequest(string Username, string Password);
