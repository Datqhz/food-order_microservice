using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.FoodCommands.CreateFood;

namespace OrderService.Consumers;

public class CreateFoodConsumer : IConsumer<CreateFood>
{
    private readonly IMediator _mediator;
    public async Task Consume(ConsumeContext<CreateFood> context)
    {
        var message = context.Message;
        try
        {
            await _mediator.Send(new CreateFoodCommand
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}
