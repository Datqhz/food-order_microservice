using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Features.Queries.OrderDetailQueries.CheckFoodIsUsed;

namespace OrderService.InternalControllers;


[ApiController]
[Route("api/v1/food")]
public class FoodInternalController : ControllerBase
{
    private readonly IMediator _mediator;

    public FoodInternalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("check-used")]
    public async Task<ActionResult<bool>> CheckUsed([FromQuery] int foodId)
    {
        var result = await _mediator.Send(new CheckFoodIsUsedQuery()
        {
            FoodId = foodId
        });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
}
