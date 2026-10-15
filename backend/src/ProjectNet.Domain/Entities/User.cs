using ProjectNet.Domain.Common;

namespace ProjectNet.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private User() { }

    public static User Create(string username, string passwordHash, string role)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new DomainException("Username is required.");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new DomainException("Password hash is required.");
        if (string.IsNullOrWhiteSpace(role)) role = "User";

        return new User
        {
            Id = Guid.NewGuid(),
            Username = username.Trim(),
            PasswordHash = passwordHash,
            Role = role.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
