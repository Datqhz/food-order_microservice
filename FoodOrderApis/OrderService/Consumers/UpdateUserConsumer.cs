using System.Text.Json;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.UserCommands.UpdateUser;

namespace OrderService.Consumers;

public class UpdateUserConsumer : IConsumer<UpdateUserInfo>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateUserConsumer> _logger;

    public UpdateUserConsumer(IMediator mediator, ILogger<UpdateUserConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UpdateUserInfo> context)
    {
        var functionName = $"{nameof(UpdateUserConsumer)} - {nameof(Consume)} =>";
        try
        {
            var message = context.Message;
            _logger.LogInformation($"{functionName} Message = {JsonSerializer.Serialize(message)}");
            await _mediator.Send(new UpdateUserCommand
            {
                Payload = new ModifyUserInput
                {
                    UserId = message.UserId,
                    DisplayName = message.DisplayName,
                    PhoneNumber = message.PhoneNumber,
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} Has error : Message = {ex.Message}");
        }
    }
}
