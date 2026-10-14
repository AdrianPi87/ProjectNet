using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.AddOrderLine;

public sealed record AddOrderLineCommand(Guid OrderId, Guid ProductId, int Quantity, decimal UnitPrice) : IRequest<OrderDto>;
