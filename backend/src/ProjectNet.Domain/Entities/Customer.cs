using ProjectNet.Domain.Common;

namespace ProjectNet.Domain.Entities;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Customer() { }

    public static Customer Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Customer name is required.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) throw new DomainException("A valid customer email is required.");

        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
