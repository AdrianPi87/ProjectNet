using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Events.Integration;
using ProjectNet.Domain.Common;

namespace ProjectNet.Application.Commands.ShipOrder;

public sealed class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;

    public ShipOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<OrderDto> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new DomainException($"Order {request.OrderId} was not found.");

        order.Ship();
        await _orderRepository.UpdateAsync(order, cancellationToken);
        await _eventBus.PublishAsync(new OrderShippedIntegrationEvent(order.Id, DateTime.UtcNow), cancellationToken);
        order.ClearDomainEvents();
        return order.ToDto();
    }
}
