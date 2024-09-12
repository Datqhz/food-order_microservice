using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Commands.OrderCommands.UpdateOrder;

public class UpdateOrderCommand : IRequest<UpdateOrderResponse>
{
    public UpdateOrderRequest Payload { get; set; }
}
