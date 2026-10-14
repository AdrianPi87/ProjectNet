namespace ProjectNet.Domain.Common;

public abstract record DomainEvent(DateTime OccurredOnUtc = default) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = OccurredOnUtc == default ? DateTime.UtcNow : OccurredOnUtc;
}
