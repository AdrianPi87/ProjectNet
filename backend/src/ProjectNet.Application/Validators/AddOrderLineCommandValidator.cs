using FluentValidation;
using ProjectNet.Application.Commands.AddOrderLine;

namespace ProjectNet.Application.Validators;

public sealed class AddOrderLineCommandValidator : AbstractValidator<AddOrderLineCommand>
{
    public AddOrderLineCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}
