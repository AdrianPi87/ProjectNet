using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;
using ProjectNet.Domain.Common;

namespace ProjectNet.Application.Commands.CancelOrder;

public sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<OrderDto> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new DomainException($"Order {request.OrderId} was not found.");

        order.Cancel();
        await _orderRepository.UpdateAsync(order, cancellationToken);
        order.ClearDomainEvents();
        return order.ToDto();
    }
}
