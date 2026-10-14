using FluentValidation;
using ProjectNet.Application.Commands.CreateProduct;

namespace ProjectNet.Application.Validators;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}
