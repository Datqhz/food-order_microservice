using FoodOrderApis.Common.Helpers;
using FoodService.Data.Requests;
using FoodService.Features.Commands.FoodCommands.CreateFood;
using FoodService.Features.Commands.FoodCommands.DeleteFood;
using FoodService.Features.Commands.FoodCommands.UpdateFood;
using FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;
using FoodService.Features.Queries.FoodQueries.GetFoodById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/food")]
public class FoodController : ControllerBase
{
    private readonly IMediator _mediator;

    public FoodController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-by-user")]
    public async Task<IActionResult> GetAllFoodByUserId([FromQuery] string userId)
    {
        var result = await _mediator.Send(new GetAllFoodByUserIdQuery
        {
            UserId = userId
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetFoodByIdQuery{Id = id});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFood([FromBody] CreateFoodInput input)
    {
        var result = await _mediator.Send(new CreateFoodCommand{Payload = input});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFood([FromBody] UpdateFoodInput input)
    {
        var result = await _mediator.Send(new UpdateFoodCommand
        {
            Payload = input
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFood([FromRoute] int id)
    {
        var resutl = await _mediator.Send(new DeleteFoodCommand{Id = id});
        return ResponseHelper.ToResponse(resutl.StatusCode, resutl.StatusText, resutl.ErrorMessage);
    }
}
