using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Queries.GetAllOrders;

public sealed record GetAllOrdersQuery : IRequest<IReadOnlyList<OrderDto>>;
