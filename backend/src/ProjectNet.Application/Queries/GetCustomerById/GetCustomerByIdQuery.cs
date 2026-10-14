using MediatR;using ProjectNet.Application.DTOs;namespace ProjectNet.Application.Queries.GetCustomerById;public sealed record GetCustomerByIdQuery(Guid Id):IRequest<CustomerDto?>;
