using FluentValidation;

namespace OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;

public class UpdateOrderDetailValidator : AbstractValidator<UpdateOrderDetailCommand>
{
    public UpdateOrderDetailValidator()
    {
        RuleFor(x => x.Payload).NotNull().WithMessage("Payload cannot be null.");
        RuleFor(x => x.Payload.Feature).NotNull().WithMessage("Feature cannot be null.").LessThan(4).GreaterThanOrEqualTo(1).WithMessage("Feature must be between 1 and 4.");
        RuleFor(x => x.Payload.Quantity).NotNull().GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        RuleFor(x => x.Payload.Price).NotNull().GreaterThanOrEqualTo(0).WithMessage("Price must be greater than 0.");
    }
}