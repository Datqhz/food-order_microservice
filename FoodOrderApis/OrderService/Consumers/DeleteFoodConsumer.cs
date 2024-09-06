using System.Text.Json;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Features.Commands.FoodCommands.DeleteFood;

namespace OrderService.Consumers;

public class DeleteFoodConsumer : IConsumer<DeleteFood>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteFoodConsumer> _logger;
    public DeleteFoodConsumer(IMediator mediator, ILogger<DeleteFoodConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<DeleteFood> context)
    {
        var functionName = $"{nameof(DeleteFoodConsumer)} - {nameof(Consume)} =>";
        try
        {
            var message = context.Message;
            _logger.LogInformation($"{functionName} Message = {JsonSerializer.Serialize(context.Message)}");
            if (message != null)
            {
                await _mediator.Send(new DeleteFoodCommand
                {
                    FoodId = message.FoodId
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} Has error : Message = {ex.Message}");
        }
    }
}
