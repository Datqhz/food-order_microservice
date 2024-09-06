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
    private readonly ILogger<CreateFoodConsumer> _logger;

    public CreateFoodConsumer(IMediator mediator, ILogger<CreateFoodConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<CreateFood> context)
    {
        var functionName = $"{nameof(CreateFoodConsumer)} - {nameof(Consume)} =>";
        
        try
        {
            var message = context.Message;
            _logger.LogInformation($"{functionName} Message = {JsonSerializer.Serialize(message)}");
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
            _logger.LogError($"{functionName} Has error = {ex.Message}");
        }
        
    }
}
