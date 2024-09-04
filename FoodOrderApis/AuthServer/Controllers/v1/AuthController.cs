using AuthServer.Data.Requests;
using AuthServer.Features.Commands.DeleteUser;
using AuthServer.Features.Commands.Login;
using AuthServer.Features.Commands.Register;
using AuthServer.Features.Commands.UpdateUser;
using FoodOrderApis.Common.Helpers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers.v1;


[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterInput input)
    {
        var result = await _mediator.Send(new RegisterCommand { Payload = input });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    [HttpPost("login")]
    public async Task<IActionResult> GetToken([FromBody] LoginInput input)
    {
        var result = await _mediator.Send(new LoginCommand { Payload = input });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }
      
    
} 