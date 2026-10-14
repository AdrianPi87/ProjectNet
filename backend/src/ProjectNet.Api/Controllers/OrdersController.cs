using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectNet.Application.Commands.AddOrderLine;
using ProjectNet.Application.Commands.CancelOrder;
using ProjectNet.Application.Commands.ConfirmOrder;
using ProjectNet.Application.Commands.CreateOrder;
using ProjectNet.Application.Commands.ShipOrder;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Queries.GetOrdersPaged;
using ProjectNet.Application.Queries.GetOrderById;

namespace ProjectNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<OrdersPageDto>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] Guid? customerId = null,
        CancellationToken cancellationToken = default)
        => Ok(await _mediator.Send(new GetOrdersPagedQuery(page, pageSize, status, customerId), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateOrderCommand(request.CustomerId), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/lines")]
    public async Task<ActionResult<OrderDto>> AddLine(Guid id, [FromBody] AddOrderLineRequest request, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new AddOrderLineCommand(id, request.ProductId, request.Quantity, request.UnitPrice), cancellationToken));

    [HttpPost("{id:guid}/confirm")]
    public async Task<ActionResult<OrderDto>> Confirm(Guid id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new ConfirmOrderCommand(id), cancellationToken));

    [HttpPost("{id:guid}/ship")]
    public async Task<ActionResult<OrderDto>> Ship(Guid id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new ShipOrderCommand(id), cancellationToken));

    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult<OrderDto>> Cancel(Guid id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new CancelOrderCommand(id), cancellationToken));
}

public sealed record CreateOrderRequest(Guid CustomerId);
public sealed record AddOrderLineRequest(Guid ProductId, int Quantity, decimal UnitPrice);



