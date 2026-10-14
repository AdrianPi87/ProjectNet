using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.ConfirmOrder;

public sealed record ConfirmOrderCommand(Guid OrderId) : IRequest<OrderDto>;
