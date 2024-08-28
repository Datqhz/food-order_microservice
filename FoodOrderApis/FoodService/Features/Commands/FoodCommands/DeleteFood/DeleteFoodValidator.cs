using FluentValidation;

namespace FoodService.Features.Commands.FoodCommands.DeleteFood;

public class DeleteFoodValidator : AbstractValidator<DeleteFoodCommand>
{
    public DeleteFoodValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty");
    }    
}
