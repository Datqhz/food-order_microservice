using FluentValidation;

namespace FoodService.Features.Commands.FoodCommands.UpdateFood;

public class UpdateFoodValidator : AbstractValidator<UpdateFoodCommand>
{
    public UpdateFoodValidator()
    {
        RuleFor(x => x.Payload).NotNull().NotEmpty().WithMessage("Please insert a food");
        RuleFor(x => x.Payload.Name).NotNull().NotEmpty().WithMessage("Food name is required");
        RuleFor(x => x.Payload.Price)
            .NotEmpty().WithMessage("Food price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Food price must be greater than or equal to 0");
        RuleFor(x => x.Payload.ImageUrl).NotNull().NotEmpty().WithMessage("Food image is required");
    }
}
