using FluentValidation;

namespace OrderService.Features.Commands.OrderCommands.UpdateOrderWithShippingInfo;

public class UpdateOrderWithShippingInfoValidator : AbstractValidator<UpdateOrderWithShippingInfoCommand>
{
    public UpdateOrderWithShippingInfoValidator()
    {
        RuleFor(x => x.Payload)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Payload cannot be null");
        RuleFor(x => x.Payload.OrderId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("OrderId is required");
        RuleFor(x => x.Payload.ShippingAddress)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Shipping address is required");
        RuleFor(x => x .Payload.ShippingFee)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(0).WithMessage("Shipping fee greather than or equal to 0");
        RuleFor(x => x.Payload.ShippingPhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage("Phone number is required");
    }
}