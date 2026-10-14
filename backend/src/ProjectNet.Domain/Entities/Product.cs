using ProjectNet.Domain.Common;

namespace ProjectNet.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Product() { }

    public static Product Create(string name, string sku, string category, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Product name is required.");
        if (unitPrice < 0) throw new DomainException("Unit price cannot be negative.");

        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Sku = sku.Trim().ToUpperInvariant(),
            Category = category.Trim(),
            UnitPrice = unitPrice,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
