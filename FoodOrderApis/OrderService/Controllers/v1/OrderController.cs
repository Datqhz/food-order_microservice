using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/order")]
public class OrderController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

}
