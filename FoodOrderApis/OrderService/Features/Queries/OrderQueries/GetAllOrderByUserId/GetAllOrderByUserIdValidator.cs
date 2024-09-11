using FluentValidation;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

public class GetAllOrderByUserIdValidator : AbstractValidator<GetAllOrderByUserIdQuery>
{
    public GetAllOrderByUserIdValidator()
    {
        RuleFor(x => x.Payload.EaterId)
            .NotNull().NotEmpty().When(x => x.Payload.MerchantId == null).WithMessage("UserId cannot be empty");
        RuleFor(x => x.Payload.MerchantId)
            .NotNull().NotEmpty().When(x => x.Payload.EaterId == null).WithMessage("UserId cannot be empty");
    }
}
