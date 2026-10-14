using MediatR;using ProjectNet.Application.DTOs;namespace ProjectNet.Application.Queries.GetProductById;public sealed record GetProductByIdQuery(Guid Id):IRequest<ProductDto?>;
