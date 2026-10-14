using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectNet.Application.Commands.CreateProduct;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Queries.GetAllProducts;
using ProjectNet.Application.Queries.GetProductById;

namespace ProjectNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetAllProductsQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateProductCommand(request.Name, request.Sku, request.Category, request.UnitPrice), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}

public sealed record CreateProductRequest(string Name, string Sku, string Category, decimal UnitPrice);
