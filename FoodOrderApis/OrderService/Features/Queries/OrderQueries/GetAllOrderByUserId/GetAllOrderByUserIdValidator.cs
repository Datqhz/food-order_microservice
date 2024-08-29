using FluentValidation;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

public class GetAllOrderByUserIdValidator : AbstractValidator<GetAllOrderByUserIdQuery>
{
    public GetAllOrderByUserIdValidator()
    {
        RuleFor(x => x.EaterId)
            .NotNull().NotEmpty().When(x => x.MerchantId == null).WithMessage("UserId cannot be empty");
        RuleFor(x => x.MerchantId)
            .NotNull().NotEmpty().When(x => x.EaterId == null).WithMessage("UserId cannot be empty");
    }
}
