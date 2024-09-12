using AuthServer.Data.Requests;
using AuthServer.Features.Commands.DeleteUser;
using AuthServer.Features.Commands.Login;
using AuthServer.Features.Commands.Register;
using AuthServer.Features.Commands.UpdateUser;
using AuthServer.Features.Queries.RevokeToken;
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
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _mediator.Send(new RegisterCommand { Payload = request });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    [HttpPost("login")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand { Payload = request });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage, result.Data);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        var result = await _mediator.Send(new RevokeTokenQuery { Payload = request });
        return ResponseHelper.ToResponse(result.StatusCode, result.StatusText, result.ErrorMessage);
    }
    
    
} 