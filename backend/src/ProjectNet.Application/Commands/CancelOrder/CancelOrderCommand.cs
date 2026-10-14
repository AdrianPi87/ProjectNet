using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest<OrderDto>;
