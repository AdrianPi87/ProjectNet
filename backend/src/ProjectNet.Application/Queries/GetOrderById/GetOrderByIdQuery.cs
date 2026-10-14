using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto?>;
