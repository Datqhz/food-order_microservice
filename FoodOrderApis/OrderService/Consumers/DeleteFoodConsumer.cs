using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Features.Commands.FoodCommands.DeleteFood;

namespace OrderService.Consumers;

public class DeleteFoodConsumer : IConsumer<DeleteFood>
{
    private readonly IMediator _mediator;

    public DeleteFoodConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<DeleteFood> context)
    {
        var message = context.Message;
        if (message != null)
        {
            await _mediator.Send(new DeleteFoodCommand
            {
                FoodId = message.FoodId
            });
        }
    }
}
