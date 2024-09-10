using FluentValidation;

namespace OrderService.Features.Queries.OrderQueries.GetInitialOrderByEaterAndMerchant;

public class GetInitialOrderByEaterAndMerchantValidator : AbstractValidator<GetInitialOrderByEaterAndMerchantQuery>
{
    public GetInitialOrderByEaterAndMerchantValidator()
    {
        RuleFor(x => x.Payload).NotNull().NotEmpty().WithMessage("Payload cannot be empty or null");
        RuleFor(x => x.Payload.EaterId).NotNull().NotEmpty().WithMessage("EaterId cannot be empty or null");
        RuleFor(x => x.Payload.MerchantId).NotNull().NotEmpty().WithMessage("MerchantId cannot be empty or null");
    }
}
