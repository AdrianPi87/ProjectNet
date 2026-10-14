using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Queries.GetOrdersPaged;

public sealed class GetOrdersPagedQueryHandler : IRequestHandler<GetOrdersPagedQuery, OrdersPageDto>
{
    private readonly IOrderRepository _repository;

    public GetOrdersPagedQueryHandler(IOrderRepository repository) => _repository = repository;

    public async Task<OrdersPageDto> Handle(GetOrdersPagedQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Status,
            request.CustomerId,
            cancellationToken);

        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new OrdersPageDto(
            items.Select(o => o.ToDto()).ToList(),
            request.Page < 1 ? 1 : request.Page,
            pageSize,
            totalCount,
            totalPages);
    }
}
