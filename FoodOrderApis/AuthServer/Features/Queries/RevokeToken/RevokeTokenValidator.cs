using FluentValidation;

namespace AuthServer.Features.Queries.RevokeToken;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenQuery>
{
    public RevokeTokenValidator()
    {
        RuleFor(x => x.Payload)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Invalid payload");
        RuleFor(x => x.Payload.AccessToken)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Invalid AccessToken");
    }
}
