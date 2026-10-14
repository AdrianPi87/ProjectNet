using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.ShipOrder;

public sealed record ShipOrderCommand(Guid OrderId) : IRequest<OrderDto>;
