using System.Net;
using AuthServer.Data.Dtos.Inputs;
using AuthServer.Features.Commands;
using IdentityModel.Client;
using MediatR;
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
    [HttpPost("get-token")]
    public async Task<IActionResult> GetToken([FromBody] GetTokenRequestInput input)
    {
        return await _mediator.Send(new GetTokenRequest { Data = input });
    }
}