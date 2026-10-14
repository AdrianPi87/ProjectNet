using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, string Sku, string Category, decimal UnitPrice) : IRequest<ProductDto>;
