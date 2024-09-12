using AuthServer.Data.Models;
using FluentValidation;

namespace AuthServer.Features.Commands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Payload)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Payload is required.");
        RuleFor(x => x.Payload.UserId)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("User id is required.");
        RuleFor(x => x.Payload.OldPassword)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Old password is required.")
            .MinimumLength(8)
            .MaximumLength(16).WithMessage("Password must be between 8 and 16 characters.");
        RuleFor(x => x.Payload.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8)
            .MaximumLength(16).WithMessage("Password must be between 8 and 16 characters.");
    }
}
