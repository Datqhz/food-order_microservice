using FluentValidation;

namespace FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;

public class GetAllFoodByUserIdValidator : AbstractValidator<GetAllFoodByUserIdQuery>
{
    public GetAllFoodByUserIdValidator()
    {
        RuleFor(q => q.UserId).NotNull().NotEmpty().WithMessage("UserId is required");
    }
}
