using System.Net;
using AuthServer.Data.Dtos.Inputs;
using AuthServer.Data.Models.Requests;
using AuthServer.Features.Commands;
using AuthServer.Features.Commands.LoginCommands;
using AuthServer.Features.Commands.RegisterCommands;
using FoodOrderApis.Common.MassTransit.Consumers;
using IdentityModel.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await _mediator.Send(new RegisterCommand { Payload = request });
    }
    [HttpPost("login")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
    {
        return await _mediator.Send(new LoginCommand { Payload = request });
    }
    
}