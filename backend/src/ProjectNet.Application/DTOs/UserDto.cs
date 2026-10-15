namespace ProjectNet.Application.DTOs;

public sealed record UserDto(Guid Id, string Username, string Role, DateTime CreatedAtUtc);
public sealed record LoginResponseDto(string Token, string Username, string Role);

public static class UserMappings
{
    public static UserDto ToDto(this ProjectNet.Domain.Entities.User user) => new(user.Id, user.Username, user.Role, user.CreatedAtUtc);
}
