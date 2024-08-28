using FluentValidation;

namespace OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;

public class CreateOrderDetailValidator : AbstractValidator<CreateOrderDetailCommand>
{
    public CreateOrderDetailValidator()
    {
        RuleFor(x => x.Payload).NotNull().WithMessage("Payload cannot be null.");
        RuleFor(x => x.Payload.Quantity).NotNull().WithMessage("Quantity cannot be null.");
        RuleFor(x => x.Payload.OrderId).NotNull().WithMessage("OrderId cannot be null.");
        RuleFor(x => x.Payload.FoodId).NotNull().WithMessage("FoodId cannot be null.");
        RuleFor(x => x.Payload.Price).NotNull().WithMessage("Price cannot be null.");
    }
}