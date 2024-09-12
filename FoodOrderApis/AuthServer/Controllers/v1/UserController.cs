using AuthServer.Data.Requests;
using AuthServer.Features.Commands.DeleteUser;
using AuthServer.Features.Commands.UpdateUser;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var result = await _mediator.Send(new UpdateUserCommand { Payload = request });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    
    [HttpDelete("delete-user/{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string userId)
    {
        var result = await _mediator.Send(new DeleteUserCommand { UserId = userId});
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }    
}
