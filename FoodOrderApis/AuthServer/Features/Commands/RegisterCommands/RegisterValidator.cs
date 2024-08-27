using System.Data;
using AuthServer.Data.Models.Requests;
using FluentValidation;

namespace AuthServer.Features.Commands.RegisterCommands;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(command => command.Payload.Displayname)
            .NotEmpty().WithMessage("Displayname cannot be empty")
            .MinimumLength(3).MaximumLength(100).WithMessage("Displayname must be between 3 and 100 characters");
        RuleFor(command => command.Payload.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(5).MaximumLength(50).WithMessage("Username must be between 5 and 50 characters");
        RuleFor(command => command.Payload.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).MaximumLength(16).WithMessage("Password must be between 8 and 16 characters");
    }
}
