using FluentValidation;
using OrderService.Data.Requests;

namespace OrderService.Features.Commands.OrderCommands.UpdateOrder;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Payload).NotNull().NotEmpty().WithMessage("Payload cannot be null or empty");
        RuleFor(x => x.Payload.OrderId).NotNull().WithMessage("OrderStatus cannot be null ");
        RuleFor(x => x.Payload.OrderStatus).NotNull().WithMessage("OrderStatus cannot be null");
    }
}
