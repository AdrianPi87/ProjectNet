using MassTransit;
using ProjectNet.Application.Abstractions;

namespace ProjectNet.Infrastructure.Messaging;

public sealed class MassTransitEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default)
        where TEvent : class
        => _publishEndpoint.Publish(integrationEvent, cancellationToken);
}
