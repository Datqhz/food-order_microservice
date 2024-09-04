using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.CreateUser;
using CustomerService.Features.Commands.UserInfoCommands.UpdateUser;
using CustomerService.Features.Queries.UserInfoQueries.GetAllMerchant;
using CustomerService.Features.Queries.UserInfoQueries.GetAllUserInfo;
using CustomerService.Features.Queries.UserInfoQueries.GetUserInfoById;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/customer")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllUserQuery());
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery() { UserId = id });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateUserInfoInput input)
    {
        var result = await _mediator.Send(new CreateUserCommand{Payload = input});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateUserInfoInput input)
    {
        var result = await _mediator.Send(new UpdateUserCommand(){Payload = input});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }

    [HttpGet("all-merchants")]
    public async Task<IActionResult> GetAllMerchants()
    {
        var result = await _mediator.Send(new GetAllMerchantQuery());
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
}