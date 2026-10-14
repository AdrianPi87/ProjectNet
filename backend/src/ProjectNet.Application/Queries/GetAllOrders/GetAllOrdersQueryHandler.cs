using MediatR;
using ProjectNet.Application.Abstractions;
using ProjectNet.Application.DTOs;

namespace ProjectNet.Application.Queries.GetAllOrders;

public sealed class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IReadOnlyList<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersQueryHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task<IReadOnlyList<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(o => o.ToDto()).ToList();
    }
}
