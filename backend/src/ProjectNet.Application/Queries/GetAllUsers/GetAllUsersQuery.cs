using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Queries.GetAllUsers;

public sealed record GetAllUsersQuery : IRequest<IReadOnlyList<UserDto>>;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserRepository _repository;
    public GetAllUsersQueryHandler(IUserRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        => (await _repository.GetAllAsync(cancellationToken)).Select(u => u.ToDto()).ToList();
}
