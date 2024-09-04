using FluentValidation;

namespace OrderService.Features.Queries.OrderDetailQueries.CheckFoodIsUsed;

public class CheckFoodIsUsedValidator : AbstractValidator<CheckFoodIsUsedQuery>
{
    public CheckFoodIsUsedValidator()
    {
        RuleFor(x => x.FoodId).NotNull().NotEmpty().WithMessage("FoodId is required");
    }
}
