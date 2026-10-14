using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(string Name, string Email) : IRequest<CustomerDto>;
