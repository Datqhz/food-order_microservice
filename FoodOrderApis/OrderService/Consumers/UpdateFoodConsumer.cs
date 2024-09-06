using System.Text.Json;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.FoodCommands.UpdateFood;

namespace OrderService.Consumers;

public class UpdateFoodConsumer : IConsumer<UpdateFood>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateFoodConsumer> _logger;

    public UpdateFoodConsumer(IMediator mediator, ILogger<UpdateFoodConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UpdateFood> context)
    {
        var functionName = $"{nameof(UpdateFoodConsumer)} - {nameof(Consume)} =>";
        try
        {
            var message = context.Message;
            _logger.LogInformation($"{functionName} Message = {JsonSerializer.Serialize(message)}");
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
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} Has error : Message = {ex.Message}");
        }
    }
}
