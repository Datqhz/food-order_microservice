using System.Text.Json;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.FoodCommands.CreateFood;

namespace OrderService.Consumers;

public class CreateFoodConsumer : IConsumer<CreateFood>
{
    private readonly IMediator _mediator;

    public CreateFoodConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<CreateFood> context)
    {
        var message = context.Message;
        Console.WriteLine(JsonSerializer.Serialize(message));
        try
        {
            var input = new ModifyFoodInput
            {
                FoodId = message.Id,
                Name = message.Name,
                Describe = message.Describe,
                ImageUrl = message.ImageUrl
            };
            await _mediator.Send(new CreateFoodCommand
            {
                Payload = input
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}
