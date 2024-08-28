using FluentValidation;

namespace OrderService.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Payload).NotNull().WithMessage("Payload cannot be null.");
        RuleFor(x => x.Payload.EaterId).NotNull().WithMessage("EaterId is required.");
        RuleFor(x => x.Payload.MerchantId).NotNull().WithMessage("MerchantId is required.");
    }
}
