using ProjectNet.Domain.Common;
using ProjectNet.Domain.Entities;

namespace ProjectNet.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void Create_ValidCustomer_CreatesPendingOrder()
    {
        var customerId = Guid.NewGuid();
        var order = Order.Create(customerId);

        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Empty(order.Lines);
        Assert.Single(order.DomainEvents);
    }

    [Fact]
    public void Create_EmptyCustomerId_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => Order.Create(Guid.Empty));
    }

    [Fact]
    public void AddLine_PendingOrder_AddsLineAndUpdatesTotal()
    {
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        order.AddLine(productId, 2, 10.5m);

        Assert.Single(order.Lines);
        Assert.Equal(21m, order.Total);
    }

    [Fact]
    public void AddLine_InvalidQuantity_Throws()
    {
        var order = Order.Create(Guid.NewGuid());
        Assert.Throws<DomainException>(() => order.AddLine(Guid.NewGuid(), 0, 10m));
    }

    [Fact]
    public void Confirm_WithLines_ConfirmsOrder()
    {
        var order = Order.Create(Guid.NewGuid());
        order.AddLine(Guid.NewGuid(), 1, 5m);
        order.ClearDomainEvents();

        order.Confirm();

        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Single(order.DomainEvents);
    }

    [Fact]
    public void Confirm_EmptyOrder_Throws()
    {
        var order = Order.Create(Guid.NewGuid());
        Assert.Throws<DomainException>(() => order.Confirm());
    }

    [Fact]
    public void Ship_OnlyConfirmed()
    {
        var order = Order.Create(Guid.NewGuid());
        order.AddLine(Guid.NewGuid(), 1, 5m);
        Assert.Throws<DomainException>(() => order.Ship());

        order.Confirm();
        order.Ship();
        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Fact]
    public void Cancel_Shipped_Throws()
    {
        var order = Order.Create(Guid.NewGuid());
        order.AddLine(Guid.NewGuid(), 1, 5m);
        order.Confirm();
        order.Ship();
        Assert.Throws<DomainException>(() => order.Cancel());
    }
}
