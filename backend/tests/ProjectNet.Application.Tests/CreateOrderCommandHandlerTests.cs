using ProjectNet.Application.Abstractions;
using ProjectNet.Application.Commands.CreateOrder;
using ProjectNet.Application.DTOs;
using ProjectNet.Application.Events.Integration;
using ProjectNet.Domain.Entities;

namespace ProjectNet.Application.Tests;

public class CreateOrderCommandHandlerTests
{
    private sealed class FakeRepository : IOrderRepository
    {
        public readonly List<Order> Orders = new();
        public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default) => Task.FromResult(Orders.FirstOrDefault(o => o.Id == id)); public Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? status, Guid? customerId, CancellationToken ct = default) => Task.FromResult(((IReadOnlyList<Order>)Orders.ToList(), Orders.Count));
        public Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default) =>
            Task.FromResult<IReadOnlyList<Order>>(Orders.ToList());
        public Task AddAsync(Order order, CancellationToken ct = default) { Orders.Add(order); return Task.CompletedTask; }
        public Task UpdateAsync(Order order, CancellationToken ct = default) => Task.CompletedTask;
    }

    private sealed class FakeEventBus : IEventBus
    {
        public readonly List<object> Published = new();
        public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken ct = default) where TEvent : class
        {
            Published.Add(integrationEvent);
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Handle_CreatesOrder_PublishesEvent_AndReturnsDto()
    {
        var repo = new FakeRepository();
        var bus = new FakeEventBus();
        var handler = new CreateOrderCommandHandler(repo, bus);
        var customerId = Guid.NewGuid();

        var result = await handler.Handle(new CreateOrderCommand(customerId), CancellationToken.None);

        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal("Pending", result.Status);
        Assert.Single(repo.Orders);
        Assert.Single(bus.Published);
        Assert.IsType<OrderCreatedIntegrationEvent>(bus.Published[0]);
    }
}

