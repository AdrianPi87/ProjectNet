using FluentValidation;
using ProjectNet.Application.Commands.CreateUser;

namespace ProjectNet.Application.Validators;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(100);
        RuleFor(x => x.Role).NotEmpty().Must(r => r is "Admin" or "User").WithMessage("Role must be Admin or User.");
    }
}
