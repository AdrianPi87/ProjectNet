using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectNet.Application.Abstractions;
using ProjectNet.Infrastructure.Messaging;
using ProjectNet.Infrastructure.Messaging.Consumers;
using ProjectNet.Infrastructure.Persistence;

namespace ProjectNet.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ?? "Server=localhost;Database=ProjectNet;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IOrderRepository, OrderRepository>(); services.AddScoped<ICustomerRepository, CustomerRepository>(); services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IEventBus, MassTransitEventBus>();

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<OrderCreatedConsumer>();

            var useRabbitMq = bool.TryParse(configuration["Messaging:UseRabbitMq"], out var enabled) && enabled;

            if (useRabbitMq)
            {
                configurator.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(
                        configuration["RabbitMq:Host"] ?? "localhost",
                        configuration["RabbitMq:VirtualHost"] ?? "/",
                        h =>
                        {
                            h.Username(configuration["RabbitMq:Username"] ?? "guest");
                            h.Password(configuration["RabbitMq:Password"] ?? "guest");
                        });

                    cfg.ConfigureEndpoints(context);
                });
            }
            else
            {
                configurator.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
            }
        });

        return services;
    }
}






