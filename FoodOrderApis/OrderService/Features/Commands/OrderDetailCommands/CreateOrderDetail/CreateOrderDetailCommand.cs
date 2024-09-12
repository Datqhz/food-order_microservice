using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;

public class CreateOrderDetailCommand : IRequest<CreateOrderDetailResponse>
{
    public CreateOrderDetailRequest Payload { get; set; }
}