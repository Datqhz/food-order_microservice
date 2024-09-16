using FoodOrderApis.Common.Helpers;
using FoodService.Data.Requests;
using FoodService.Features.Commands.FoodCommands.CreateFoodCommands;
using FoodService.Features.Commands.FoodCommands.DeleteFoodCommands;
using FoodService.Features.Commands.FoodCommands.UpdateFoodCommands;
using FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;
using FoodService.Features.Queries.FoodQueries.GetFoodById;
using FoodService.Features.Queries.FoodQueries.SearchFoodByName;
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
    [Authorize(Policy = "FoodRead")]
    [HttpGet("get-by-user")]
    public async Task<IActionResult> GetAllFoodByUserId([FromQuery] GetAllFoodByUserIdRequest request)
    {
        var result = await _mediator.Send(new GetAllFoodByUserIdQuery
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetFoodByIdQuery{Id = id});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }

    [Authorize(Policy = "FoodWrite")]
    [HttpPost]
    public async Task<IActionResult> CreateFood([FromBody] CreateFoodRequest request)
    {
        var result = await _mediator.Send(new CreateFoodCommand{Payload = request});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFood([FromBody] UpdateFoodRequest request)
    {
        var result = await _mediator.Send(new UpdateFoodCommand
        {
            Payload = request
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFood([FromRoute] int id)
    {
        var resutl = await _mediator.Send(new DeleteFoodCommand{Id = id});
        return ResponseHelper.ToResponse(resutl.StatusCode, resutl.StatusText, resutl.ErrorMessage);
    }
    
    [HttpGet("search-by-name")]
    public async Task<IActionResult> GetFoodById([FromQuery] SearchFoodsByNameRequest request)
    {
        var result = await _mediator.Send(new SearchFoodsByNameQuery
        {
            Payload = request
        });
        return ResponseHelper.ToPaginationResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data, result.Paging);
    }
}
