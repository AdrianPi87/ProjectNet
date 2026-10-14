namespace ProjectNet.Application.DTOs;

public sealed record OrderLineDto(Guid ProductId, int Quantity, decimal UnitPrice, decimal LineTotal);

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal Total,
    DateTime CreatedAtUtc,
    IReadOnlyList<OrderLineDto> Lines);

public static class OrderMappings
{
    public static OrderDto ToDto(this ProjectNet.Domain.Entities.Order order) =>
        new(
            order.Id,
            order.CustomerId,
            order.Status.ToString(),
            order.Total,
            order.CreatedAtUtc,
            order.Lines.Select(l => new OrderLineDto(l.ProductId, l.Quantity, l.UnitPrice, l.LineTotal)).ToList());
}
