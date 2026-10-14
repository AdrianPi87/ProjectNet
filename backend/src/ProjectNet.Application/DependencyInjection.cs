using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProjectNet.Application.Behaviors;
using ProjectNet.Application.Commands.AddOrderLine;
using ProjectNet.Application.Commands.CreateCustomer; using ProjectNet.Application.Commands.CreateOrder; using ProjectNet.Application.Commands.CreateProduct;
using ProjectNet.Application.Validators;

namespace ProjectNet.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>(); services.AddTransient<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>(); services.AddTransient<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddTransient<IValidator<AddOrderLineCommand>, AddOrderLineCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}

