using FluentValidation;

namespace OrderService.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Payload).NotNull().WithMessage("Payload cannot be null.");
        RuleFor(x => x.Payload.EaterId).NotNull().NotEmpty().WithMessage("EaterId is required.");
        RuleFor(x => x.Payload.MerchantId).NotNull().NotEmpty().WithMessage("MerchantId is required.");
    }
}
