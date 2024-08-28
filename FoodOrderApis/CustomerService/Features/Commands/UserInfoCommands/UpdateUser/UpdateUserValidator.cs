using FluentValidation;

namespace CustomerService.Features.Commands.UserInfoCommands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Payload).NotNull().NotEmpty().WithMessage("Payload cannot be empty");
        RuleFor(x => x.Payload.Id).NotNull().NotEmpty().WithMessage("User id cannot be empty");
        RuleFor(x => x.Payload.DisplayName).NotNull().NotEmpty().WithMessage("Display name cannot be empty");
        RuleFor(x => x.Payload.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone number cannot be empty");
        RuleFor(x => x.Payload.IsActive).NotNull().WithMessage("Status cannot be empty");
    }
}
