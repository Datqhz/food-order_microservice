using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;

public class UpdateOrderDetailCommand : IRequest<UpdateOrderDetailResponse>
{
    public UpdateOrderDetailInput Payload { get; set; }
}