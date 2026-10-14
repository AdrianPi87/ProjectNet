using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Events.Integration;
using ProjectNet.Domain.Common;

namespace ProjectNet.Application.Commands.ConfirmOrder;

public sealed class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;

    public ConfirmOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<OrderDto> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new DomainException($"Order {request.OrderId} was not found.");

        order.Confirm();
        await _orderRepository.UpdateAsync(order, cancellationToken);
        await _eventBus.PublishAsync(new OrderConfirmedIntegrationEvent(order.Id, DateTime.UtcNow), cancellationToken);
        order.ClearDomainEvents();
        return order.ToDto();
    }
}
