using FluentValidation;
using FoodOrderApis.Common.ValidationRules;

namespace CustomerService.Features.Commands.UserInfoCommands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Payload)
            .NotNull().NotEmpty().WithMessage("Payload cannot be empty");
        RuleFor(x => x.Payload.Id)
            .NotNull().NotEmpty().WithMessage("User id cannot be empty");
        RuleFor(command => command.Payload.DisplayName)
            .NotNull().NotEmpty().WithMessage("DisplayName is required")
            .MinimumLength(3).MaximumLength(100).WithMessage("DisplayName must be between 3 and 100 characters");
        RuleFor(command => command.Payload.PhoneNumber)
            .NotNull().NotEmpty().WithMessage("PhoneNumber is required")
            .PhoneNumber().WithMessage("Invalid phone number");
    }
}
