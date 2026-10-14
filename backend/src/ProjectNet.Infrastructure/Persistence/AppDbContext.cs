using Microsoft.EntityFrameworkCore;
using ProjectNet.Domain.Entities;

namespace ProjectNet.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(150);
            entity.Property(c => c.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Category).IsRequired().HasMaxLength(50);
            entity.Property(p => p.UnitPrice).HasPrecision(18, 2);
            entity.Property(p => p.CreatedAtUtc).IsRequired();
            entity.HasIndex(p => p.Sku).IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.CustomerId).IsRequired();
            entity.Property(o => o.Status).HasConversion<string>().IsRequired();
            entity.Property(o => o.CreatedAtUtc).IsRequired();

            entity.OwnsMany(o => o.Lines, line =>
            {
                line.WithOwner().HasForeignKey("OrderId");
                line.Property(l => l.ProductId).IsRequired();
                line.Property(l => l.Quantity).IsRequired();
                line.Property(l => l.UnitPrice).HasPrecision(18, 2);
                line.HasKey("OrderId", "ProductId");
            });
        });
    }
}
