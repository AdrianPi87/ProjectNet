using MassTransit;
using Microsoft.Extensions.Logging;
using ProjectNet.Application.Events.Integration;

namespace ProjectNet.Infrastructure.Messaging.Consumers;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreatedIntegrationEvent>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Order created event consumed. OrderId={OrderId}, CustomerId={CustomerId}, OccurredOnUtc={OccurredOnUtc}",
            message.OrderId, message.CustomerId, message.OccurredOnUtc);
        return Task.CompletedTask;
    }
}
