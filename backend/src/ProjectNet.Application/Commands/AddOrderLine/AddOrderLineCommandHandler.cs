using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;
using ProjectNet.Domain.Common;

namespace ProjectNet.Application.Commands.AddOrderLine;

public sealed class AddOrderLineCommandHandler : IRequestHandler<AddOrderLineCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderLineCommandHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<OrderDto> Handle(AddOrderLineCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new DomainException($"Order {request.OrderId} was not found.");

        order.AddLine(request.ProductId, request.Quantity, request.UnitPrice);
        await _orderRepository.UpdateAsync(order, cancellationToken);
        order.ClearDomainEvents();
        return order.ToDto();
    }
}
