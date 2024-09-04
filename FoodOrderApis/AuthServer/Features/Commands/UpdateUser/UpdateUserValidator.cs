using AuthServer.Data.Models;
using FluentValidation;

namespace AuthServer.Features.Commands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Payload).NotNull().WithMessage("Payload is required.");
        RuleFor(x => x.Payload.UserId).NotNull().NotEmpty().WithMessage("User id is required.");
        RuleFor(x => x.Payload.Password)
            .NotNull().NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8)
            .MaximumLength(16).WithMessage("Password must be between 8 and 16 characters.");
    }
}
