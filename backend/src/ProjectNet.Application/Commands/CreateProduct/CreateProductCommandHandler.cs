using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;
using ProjectNet.Domain.Entities;

namespace ProjectNet.Application.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository) => _repository = repository;

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Sku, request.Category, request.UnitPrice);
        await _repository.AddAsync(product, cancellationToken);
        return product.ToDto();
    }
}
