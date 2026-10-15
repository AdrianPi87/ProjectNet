using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;
