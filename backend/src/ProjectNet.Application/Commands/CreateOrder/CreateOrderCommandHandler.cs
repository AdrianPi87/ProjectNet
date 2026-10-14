using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Events.Integration;
using ProjectNet.Domain.Entities;

namespace ProjectNet.Application.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.CustomerId);
        await _orderRepository.AddAsync(order, cancellationToken);
        await _eventBus.PublishAsync(
            new OrderCreatedIntegrationEvent(order.Id, order.CustomerId, DateTime.UtcNow),
            cancellationToken);

        order.ClearDomainEvents();
        return order.ToDto();
    }
}
