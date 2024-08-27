using FluentValidation;

namespace AuthServer.Features.Commands.LoginCommands;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(command => command.Payload).NotNull().WithMessage("Please provide your infomation");
        RuleFor(command => command.Payload.Username)
            .NotEmpty().WithMessage("Please provide your username")
            .MinimumLength(3).MaximumLength(100).WithMessage("Username must be between 3 and 100 characters");
        RuleFor(command => command.Payload.Password)
            .NotEmpty().WithMessage("Please provide your password")
            .MinimumLength(8).MaximumLength(16).WithMessage("Password must be between 8 and 16 characters");
        RuleFor(command => command.Payload.Scope)
            .NotEmpty().WithMessage("Please provide scopes you want");
    }
}
