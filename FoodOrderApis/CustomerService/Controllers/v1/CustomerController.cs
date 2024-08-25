using CustomerService.Data.Models.Dtos.Inputs;
using CustomerService.Features.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerInput input)
    {
        var result = await _mediator.Send(new CreateCustomerRequest{Data = input});
        return result;
    }
}