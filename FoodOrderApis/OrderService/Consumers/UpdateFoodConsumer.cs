using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.FoodCommands.UpdateFood;

namespace OrderService.Consumers;

public class UpdateFoodConsumer : IConsumer<UpdateFood>
{
    private readonly IMediator _mediator;

    public UpdateFoodConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<UpdateFood> context)
    {
        var message = context.Message;
        await _mediator.Send(new UpdateFoodCommand
        {
            Payload = new ModifyFoodInput
            {
                FoodId = message.Id,
                Name = message.Name,
                Describe = message.Describe,
                ImageUrl = message.ImageUrl
            }
        });
    }
}
