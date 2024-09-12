using FluentValidation;

namespace AuthServer.Features.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(command => command.Payload)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Please provide your infomation");
        RuleFor(command => command.Payload.Username)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Please provide your username")
            .MinimumLength(5).MaximumLength(50).WithMessage("Username must be between 5 and 50 characters");
        RuleFor(command => command.Payload.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Please provide your password")
            .MinimumLength(8).MaximumLength(16).WithMessage("Password must be between 8 and 16 characters");
    }
}
