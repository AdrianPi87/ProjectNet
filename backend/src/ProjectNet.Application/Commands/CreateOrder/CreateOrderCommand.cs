using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId) : IRequest<OrderDto>;
