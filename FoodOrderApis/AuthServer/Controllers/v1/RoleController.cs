using AuthServer.Features.Queries.GetAllRoles;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers.v1;

[ApiController]
[Route("api/v1/role")]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
}