using FluentValidation;

namespace FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;

public class GetAllFoodByUserIdValidator : AbstractValidator<GetAllFoodByUserIdQuery>
{
    public GetAllFoodByUserIdValidator()
    {
        RuleFor(q => q.Payload.UserId).NotNull().NotEmpty().WithMessage("UserId is required");
    }
}
