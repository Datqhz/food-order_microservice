using FluentValidation;

namespace CustomerService.Features.Queries.UserInfoQueries.GetUserInfoById;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(q => q.UserId).NotNull().NotEmpty().WithMessage("User id cannot be empty");
    }
}
