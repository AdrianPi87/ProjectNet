namespace ProjectNet.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
