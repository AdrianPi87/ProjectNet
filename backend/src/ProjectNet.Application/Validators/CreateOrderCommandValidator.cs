using FluentValidation;
using ProjectNet.Application.Commands.CreateOrder;

namespace ProjectNet.Application.Validators;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
