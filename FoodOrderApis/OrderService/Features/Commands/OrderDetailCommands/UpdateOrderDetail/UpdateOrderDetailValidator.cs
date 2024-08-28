using FluentValidation;

namespace OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;

public class UpdateOrderDetailValidator : AbstractValidator<UpdateOrderDetailCommand>
{
    public UpdateOrderDetailValidator()
    {
        RuleFor(x => x.Payload).NotNull().WithMessage("Payload cannot be null.");
        RuleFor(x => x.Payload.Feature).NotNull().WithMessage("Feature cannot be null.");
        RuleFor(x => x.Payload.Quantity).NotNull().WithMessage("Quantity cannot be null.");
        RuleFor(x => x.Payload.Price).NotNull().WithMessage("Price cannot be null.");
    }
}