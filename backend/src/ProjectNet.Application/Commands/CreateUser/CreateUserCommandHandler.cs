using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;
using ProjectNet.Domain.Entities;

namespace ProjectNet.Application.Commands.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.GetByUsernameAsync(request.Username, cancellationToken) is not null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var user = User.Create(request.Username, _passwordHasher.Hash(request.Password), request.Role);
        await _repository.AddAsync(user, cancellationToken);
        return user.ToDto();
    }
}
