using FluentValidation;
using FoodOrderApis.Common.ValidationRules;

namespace CustomerService.Features.Commands.UserInfoCommands.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(command => command.Payload.DisplayName)
            .NotNull().NotEmpty().WithMessage("Displayname cannot be empty")
            .MinimumLength(3).MaximumLength(100).WithMessage("Displayname must be between 3 and 100 characters");
        RuleFor(command => command.Payload.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(5).MaximumLength(50).WithMessage("Username must be between 5 and 50 characters");
        RuleFor(command => command.Payload.Role)
            .NotEmpty().WithMessage("Role cannot be empty");
        RuleFor(command => command.Payload.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty")
            .PhoneNumber().WithMessage("Phone number invalid");
        RuleFor(command => command.Payload.UserId)
            .NotNull().NotEmpty().WithMessage("User id is required");
    }
}