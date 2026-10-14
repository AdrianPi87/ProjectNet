using MediatR;using ProjectNet.Application.DTOs;namespace ProjectNet.Application.Queries.GetAllCustomers;public sealed record GetAllCustomersQuery : IRequest<IReadOnlyList<CustomerDto>>;
