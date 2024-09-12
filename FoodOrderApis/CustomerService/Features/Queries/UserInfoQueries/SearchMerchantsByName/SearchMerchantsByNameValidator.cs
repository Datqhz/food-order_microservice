using FluentValidation;

namespace CustomerService.Features.Queries.UserInfoQueries.SearchMerchantsByName;

public class SearchMerchantsByNameValidator : AbstractValidator<SearchMerchantsByNameQuery>
{
    public SearchMerchantsByNameValidator()
    {
        RuleFor(x => x.Payload)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Payload cannot be empty");
        RuleFor(x => x.Payload.Keyword)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Keyword cannot be empty");
    }
}
