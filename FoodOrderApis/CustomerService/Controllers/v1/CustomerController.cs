using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.CreateUser;
using CustomerService.Features.Commands.UserInfoCommands.UpdateUser;
using CustomerService.Features.Queries.UserInfoQueries.GetAllMerchant;
using CustomerService.Features.Queries.UserInfoQueries.GetAllUserInfo;
using CustomerService.Features.Queries.UserInfoQueries.GetUserInfoById;
using CustomerService.Features.Queries.UserInfoQueries.SearchMerchantsByName;
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
    public async Task<IActionResult> GetAll([FromQuery] GetAllUserInfoRequest request)
    {
        var result = await _mediator.Send(new GetAllUserQuery
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery() { UserId = id });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateUserInfoRequest request)
    {
        var result = await _mediator.Send(new UpdateUserCommand(){Payload = request});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }


    [HttpGet("all-merchants")]
    public async Task<IActionResult> GetAllMerchants([FromQuery] GetAllMerchantRequest? input)

    {
        var result = await _mediator.Send(new GetAllMerchantQuery
        {
            Payload = input
        });
        return ResponseHelper.ToPaginationResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data, result.Paging);
    }
    [HttpGet("search-by-name")]
    public async Task<IActionResult> SearchMerchantsByName([FromQuery] SearchMerchantsByNameRequest request)
    {
        var result = await _mediator.Send(new SearchMerchantsByNameQuery
        {
            Payload = request
        });
        return ResponseHelper.ToPaginationResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data, result.Paging);
    }
}