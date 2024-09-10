using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Commands.OrderDetailCommands.ModifyMultipleOrderDetail;

public class ModifyMultipleOrderDetailCommand : IRequest<ModifyMultipleOrderDetailResponse>
{
    public List<ModifyOrderDetailInput> Payload { get; set; }
}