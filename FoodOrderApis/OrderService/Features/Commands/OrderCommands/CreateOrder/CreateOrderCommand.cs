using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public CreateOrderRequest Payload { get; set; }
}
