namespace ProjectNet.Application.DTOs;

public sealed record CustomerDto(Guid Id, string Name, string Email, DateTime CreatedAtUtc);
public sealed record ProductDto(Guid Id, string Name, string Sku, string Category, decimal UnitPrice);

public static class CustomerMappings
{
    public static CustomerDto ToDto(this ProjectNet.Domain.Entities.Customer customer) =>
        new(customer.Id, customer.Name, customer.Email, customer.CreatedAtUtc);
}

public static class ProductMappings
{
    public static ProductDto ToDto(this ProjectNet.Domain.Entities.Product product) =>
        new(product.Id, product.Name, product.Sku, product.Category, product.UnitPrice);
}
