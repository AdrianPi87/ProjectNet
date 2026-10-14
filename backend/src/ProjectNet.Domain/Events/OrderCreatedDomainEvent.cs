using ProjectNet.Domain.Common;

namespace ProjectNet.Domain.Events;

public sealed record OrderCreatedDomainEvent(Guid OrderId, Guid CustomerId) : DomainEvent;
public sealed record OrderConfirmedDomainEvent(Guid OrderId) : DomainEvent;
public sealed record OrderShippedDomainEvent(Guid OrderId) : DomainEvent;
public sealed record OrderCancelledDomainEvent(Guid OrderId) : DomainEvent;
