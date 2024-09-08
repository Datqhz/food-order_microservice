using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;
using OrderService.Features.Commands.OrderDetailCommands.ModifyMultipleOrderDetail;
using OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;
using OrderService.Features.Queries.OrderDetailQueries.GetAllOrderDetailsByOrderId;

namespace OrderService.Controllers.v1;

[ApiController]
[Route("api/v1/order-detail")]
public class OrderDetailController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderDetailController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "OrderRead")]
    [HttpGet("get-by-order/{orderId}")]
    public async Task<IActionResult> GetOrderDetailByOrderIdAsync(int orderId)
    {
        var result = await _mediator.Send(new GetAllOrderDetailsByOrderIdQuery
        {
            OrderId = orderId
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
    [Authorize(Policy = "OrderWrite")]
    [HttpPost]
    public async Task<IActionResult> CreateOrderDetail([FromBody] CreateOrderDetailInput input)
    {
        var result = await _mediator.Send(new CreateOrderDetailCommand
        {
            Payload = input
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    [Authorize(Policy = "OrderWrite")]
    [HttpPut]
    public async Task<IActionResult> UpdateOrderDetail([FromBody] UpdateOrderDetailInput input)
    {
        var result = await _mediator.Send(new UpdateOrderDetailCommand
        {
            Payload = input
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    [Authorize(Policy = "OrderWrite")]
    [HttpPost("modify-multiple")]
    public async Task<IActionResult> ModifyMultipleOrderDetail([FromBody] List<UpdateOrderDetailInput> input)
    {
        var result = await _mediator.Send(new ModifyMultipleOrderDetailCommand
        {
            Payload = input
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
}
