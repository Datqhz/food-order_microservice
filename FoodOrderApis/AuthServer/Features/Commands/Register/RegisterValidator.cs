using System.Data;
using AuthServer.Data.Requests;
using FluentValidation;
using FoodOrderApis.Common.ValidationRules;

namespace AuthServer.Features.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(command => command.Payload.Displayname)
            .NotNull().NotEmpty().WithMessage("Displayname is required")
            .MinimumLength(3).MaximumLength(100).WithMessage("DisplayName must be between 3 and 100 characters");
        RuleFor(command => command.Payload.Username)
            .NotNull().NotEmpty().WithMessage("Username is required")
            .MinimumLength(5).MaximumLength(50).WithMessage("Username must be between 5 and 50 characters");
        RuleFor(command => command.Payload.Password)
            .NotNull().NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).MaximumLength(16).WithMessage("Password must be between 8 and 16 characters");
        RuleFor(command => command.Payload.PhoneNumber)
            .NotNull().NotEmpty().WithMessage("PhoneNumber is required")
            .PhoneNumber().WithMessage("Invalid phone number");
    }
}
