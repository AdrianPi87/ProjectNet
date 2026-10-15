using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.CreateUser;

public sealed record CreateUserCommand(string Username, string Password, string Role) : IRequest<UserDto>;
