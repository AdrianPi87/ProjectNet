using MediatR;using ProjectNet.Application.DTOs;namespace ProjectNet.Application.Queries.GetAllProducts;public sealed record GetAllProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
