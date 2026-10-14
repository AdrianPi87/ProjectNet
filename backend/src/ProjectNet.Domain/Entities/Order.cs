using ProjectNet.Domain.Common;
using ProjectNet.Domain.Events;

namespace ProjectNet.Domain.Entities;

public sealed class Order : AggregateRoot<Guid>
{
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public List<OrderLine> Lines { get; private set; } = new();
    public decimal Total => Lines.Sum(l => l.LineTotal);
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Order() { }

    public static Order Create(Guid customerId, Guid? id = null)
    {
        if (customerId == Guid.Empty) throw new DomainException("CustomerId is required.");
        var order = new Order
        {
            Id = id ?? Guid.NewGuid(),
            CustomerId = customerId,
            Status = OrderStatus.Pending
        };
        order.AddDomainEvent(new OrderCreatedDomainEvent(order.Id, customerId));
        return order;
    }

    public void AddLine(Guid productId, int quantity, decimal unitPrice)
    {
        if (Status != OrderStatus.Pending) throw new DomainException("Only pending orders can be modified.");
        if (productId == Guid.Empty) throw new DomainException("ProductId is required.");
        if (quantity <= 0) throw new DomainException("Quantity must be positive.");
        if (unitPrice < 0) throw new DomainException("Unit price cannot be negative.");

        Lines.Add(new OrderLine(productId, quantity, unitPrice));
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending) throw new DomainException("Only pending orders can be confirmed.");
        if (!Lines.Any()) throw new DomainException("Cannot confirm an empty order.");
        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmedDomainEvent(Id));
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed) throw new DomainException("Only confirmed orders can be shipped.");
        Status = OrderStatus.Shipped;
        AddDomainEvent(new OrderShippedDomainEvent(Id));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped) throw new DomainException("Shipped orders cannot be cancelled.");
        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelledDomainEvent(Id));
    }
}
