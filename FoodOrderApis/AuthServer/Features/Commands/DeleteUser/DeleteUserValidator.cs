using FluentValidation;

namespace AuthServer.Features.Commands.DeleteUser;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(p => p.UserId)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}
