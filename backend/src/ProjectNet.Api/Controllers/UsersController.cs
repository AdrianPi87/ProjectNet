using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectNet.Application.Commands.CreateUser;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Queries.GetAllUsers;

namespace ProjectNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetAllUsersQuery(), cancellationToken));

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new CreateUserCommand(request.Username, request.Password, request.Role), cancellationToken));
}

public sealed record CreateUserRequest(string Username, string Password, string Role);
