using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ProjectNet.Domain.Common;
using ProjectNet.Domain.Entities;
using ProjectNet.Infrastructure.Persistence;

namespace ProjectNet.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await SeedUsersAsync(db);
        await SeedCustomersAsync(db);
        await SeedProductsAsync(db);
        await SeedOrdersAsync(db);
    }

    private static async Task SeedUsersAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        var users = new[]
        {
            User.Create("admin", BCrypt.Net.BCrypt.HashPassword("Admin123!"), "Admin"),
            User.Create("user", BCrypt.Net.BCrypt.HashPassword("User123!"), "User")
        };

        await db.Users.AddRangeAsync(users);
        await db.SaveChangesAsync();
    }

    private static async Task SeedCustomersAsync(AppDbContext db)
    {
        if (await db.Customers.AnyAsync()) return;

        var names = new[]
        {
            ("Ana Torres", "ana.torres@example.com"),
            ("Bruno Sanchez", "bruno.sanchez@example.com"),
            ("Carla Munoz", "carla.munoz@example.com"),
            ("Diego Ramirez", "diego.ramirez@example.com"),
            ("Elena Vega", "elena.vega@example.com"),
            ("Fernando Gomez", "fernando.gomez@example.com"),
            ("Gabriela Lopez", "gabriela.lopez@example.com"),
            ("Hector Herrera", "hector.herrera@example.com"),
            ("Irene Castillo", "irene.castillo@example.com"),
            ("Javier Ortega", "javier.ortega@example.com")
        };

        var customers = names.Select(x => Customer.Create(x.Item1, x.Item2)).ToList();
        await db.Customers.AddRangeAsync(customers);
        await db.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(AppDbContext db)
    {
        if (await db.Products.AnyAsync()) return;

        var products = new (string Name, string Sku, string Category, decimal Price)[]
        {
            ("Clean Architecture in .NET", "BOOK-CA", "Books", 39.90m),
            ("Domain-Driven Design (Blue Book)", "BOOK-DDD", "Books", 59.00m),
            ("Mechanical Keyboard (USB-C)", "HW-KB87", "Hardware", 89.99m),
            ("Ergonomic Chair", "FURN-CHR", "Furniture", 199.90m),
            ("Wireless Mouse", "HW-MSE", "Hardware", 45.50m),
            ("4K Monitor 27\"", "HW-MON27", "Hardware", 329.00m),
            ("Coffee Mug (Dev edition)", "MUG-DEV", "Merch", 12.90m),
            ("Backpack (Tech)", "BAG-TECH", "Merch", 69.90m),
            ("Cloud Subscription (1y)", "SUB-CLOUD", "Services", 120.00m),
            ("Code Review Service (1h)", "SVC-CR", "Services", 75.00m)
        };

        var seeded = products.Select(p => Product.Create(p.Name, p.Sku, p.Category, p.Price)).ToList();
        await db.Products.AddRangeAsync(seeded);
        await db.SaveChangesAsync();
    }

    private static async Task SeedOrdersAsync(AppDbContext db)
    {
        if (await db.Orders.AnyAsync()) return;

        var customers = await db.Customers.ToListAsync();
        var products = await db.Products.ToListAsync();
        if (!customers.Any() || !products.Any()) return;

        var statuses = new[] { OrderStatus.Pending, OrderStatus.Pending, OrderStatus.Confirmed, OrderStatus.Shipped };
        var random = new Random(42);

        var orders = new List<Order>();
        for (int i = 0; i < 10; i++)
        {
            var customer = customers[random.Next(customers.Count)];
            var order = Order.Create(customer.Id);

            int lines = random.Next(1, 4);
            var chosenProducts = products.OrderBy(_ => random.Next()).Take(lines).ToList();
            foreach (var product in chosenProducts)
            {
                order.AddLine(product.Id, random.Next(1, 4), product.UnitPrice);
            }

            var status = statuses[random.Next(statuses.Length)];
            if (status == OrderStatus.Confirmed) order.Confirm();
            if (status == OrderStatus.Shipped)
            {
                order.Confirm();
                order.Ship();
            }

            orders.Add(order);
        }

        await db.Orders.AddRangeAsync(orders);
        await db.SaveChangesAsync();
    }
}
