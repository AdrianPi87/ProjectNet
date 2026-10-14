namespace ProjectNet.Domain.Entities;

public sealed record OrderLine(Guid ProductId, int Quantity, decimal UnitPrice)
{
    public decimal LineTotal => Quantity * UnitPrice;
}
