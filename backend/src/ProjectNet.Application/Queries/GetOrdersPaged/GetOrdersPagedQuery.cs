using MediatR;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Queries.GetOrdersPaged;

public sealed record GetOrdersPagedQuery(int Page, int PageSize, string? Status, Guid? CustomerId) : IRequest<OrdersPageDto>;
