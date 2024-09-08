using FluentValidation;

namespace OrderService.Features.Commands.OrderDetailCommands.ModifyMultipleOrderDetail;

public class ModifyMultipleOrderDetailValidator : AbstractValidator<ModifyMultipleOrderDetailCommand>
{
    public ModifyMultipleOrderDetailValidator()
    {
        RuleFor(x => x.Payload)
            .NotNull().NotEmpty().WithMessage("Payload cannot be empty or null")
            .ForEach(itemRule =>
            {
                itemRule
                    .Must(item => item.Feature != null).WithMessage("Feature cannot be null")
                    .Must(item => item.Feature >= 1 && item.Feature <= 3).WithMessage("Feature must be between 1 and 3");
                itemRule
                    .Must(item => item.Price >= 0).WithMessage("Price must be greater than or equal 0");
                itemRule
                    .Must(item => item.Quantity > 0).WithMessage("Quantity must be greater than 0");
            });

    }
}