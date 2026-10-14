using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectNet.Application.Commands.CreateCustomer;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Queries.GetAllCustomers;
using ProjectNet.Application.Queries.GetCustomerById;

namespace ProjectNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetAllCustomersQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(request.Name, request.Email), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}

public sealed record CreateCustomerRequest(string Name, string Email);
