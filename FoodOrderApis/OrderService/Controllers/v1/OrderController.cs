using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderCommands.CreateOrder;
using OrderService.Features.Commands.OrderCommands.UpdateOrder;
using OrderService.Features.Commands.OrderCommands.UpdateOrderWithShippingInfo;
using OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;
using OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;
using OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;
using OrderService.Features.Queries.OrderQueries.GetInitialOrderByEaterAndMerchant;
using OrderService.Features.Queries.OrderQueries.GetOrderById;

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
    
    [Authorize(Policy = "OrderRead")]
    [HttpGet("get-by-user")]
    public async Task<IActionResult> GetOrderByUserIdAsync([FromQuery] GetAllOrderByUserIdRequest request)
    {
        var result = await _mediator.Send(new GetAllOrderByUserIdQuery
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
    
    [Authorize(Policy = "OrderRead")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderByOrderId([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery
        {
            Id = id
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
    [Authorize(Policy = "OrderWrite")]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var result = await _mediator.Send(new CreateOrderCommand
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    [Authorize(Policy = "OrderWrite")]
    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
    {
        var result = await _mediator.Send(new UpdateOrderCommand
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    
    [Authorize(Policy = "OrderWrite")]
    [HttpPut("update-shipping-info")]
    public async Task<IActionResult> UpdateOrderWithShippingInfo([FromBody] UpdateOrderWithShippingInfoRequest request)
    {
        var result = await _mediator.Send(new UpdateOrderWithShippingInfoCommand
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    
    [Authorize(Policy = "OrderWrite")]
    [HttpGet("get-by-eater-merchant")]
    public async Task<IActionResult> GetOrderDetailByEaterMerchantAsync([FromQuery] GetInitialOrderByEaterAndMerchantRequest request)
    {
        var result = await _mediator.Send(new GetInitialOrderByEaterAndMerchantQuery()
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
}
