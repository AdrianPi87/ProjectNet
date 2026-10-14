namespace ProjectNet.Application.Events.Integration;

public sealed record OrderCreatedIntegrationEvent(Guid OrderId, Guid CustomerId, DateTime OccurredOnUtc);
public sealed record OrderConfirmedIntegrationEvent(Guid OrderId, DateTime OccurredOnUtc);
public sealed record OrderShippedIntegrationEvent(Guid OrderId, DateTime OccurredOnUtc);
