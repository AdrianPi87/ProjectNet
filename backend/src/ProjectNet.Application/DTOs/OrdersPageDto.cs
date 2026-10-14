namespace ProjectNet.Application.DTOs; public sealed record OrdersPageDto(IReadOnlyList<OrderDto> Items, int Page, int PageSize, int TotalCount, int TotalPages);
