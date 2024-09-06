using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderCommands.CreateOrder;
using OrderService.Features.Commands.OrderCommands.UpdateOrder;
using OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;
using OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;
using OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

namespace OrderService.Controllers.v1;

[ApiController]
[Route("api/v1/order")]
public class OrderController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Roles = "OrderRead")]
    [HttpGet("get-by-user")]
    public async Task<IActionResult> GetOrderByOrderIdAsync([FromQuery] string? eaterId = null, [FromQuery] string? merchantId = null)
    {
        var result = await _mediator.Send(new GetAllOrderByUserIdQuery
        {
            EaterId = eaterId,
            MerchantId = merchantId
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
    [Authorize(Policy = "OrderWrite")]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderInput input)
    {
        var result = await _mediator.Send(new CreateOrderCommand
        {
            Payload = input
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    [Authorize(Policy = "OrderWrite")]
    [HttpPut]
    public async Task<IActionResult> UpdateOrderDetail([FromBody] UpdateOrderInput input)
    {
        var result = await _mediator.Send(new UpdateOrderCommand
        {
            Payload = input
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
}
