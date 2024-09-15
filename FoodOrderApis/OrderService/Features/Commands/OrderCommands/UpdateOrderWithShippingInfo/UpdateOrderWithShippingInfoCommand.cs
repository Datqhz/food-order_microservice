using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Commands.OrderCommands.UpdateOrderWithShippingInfo;

public class UpdateOrderWithShippingInfoCommand : IRequest<UpdateOrderResponse>
{
    public UpdateOrderWithShippingInfoRequest Payload { get; set; }
}