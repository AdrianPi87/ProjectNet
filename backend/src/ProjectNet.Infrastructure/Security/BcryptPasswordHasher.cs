using ProjectNet.Application.Abstractions;

namespace ProjectNet.Infrastructure.Security;

public sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, 11);
    public bool Verify(string passwordHash, string password) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
