using System.Text.Json;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodService.Data.Requests;
using FoodService.Features.Commands.UserCommands.UpdateUser;
using MassTransit;
using MediatR;

namespace FoodService.Consumers;

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
            _logger.LogInformation($"{functionName} Messsage = {JsonSerializer.Serialize(message)}");
            await _mediator.Send(new UpdateUserCommand
            {
                Payload = new ModifyUserRequest
                {
                    UserId = message.UserId,
                    DisplayName = message.DisplayName,
                    PhoneNumber = message.PhoneNumber
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} Has error : Message = {ex.Message}");
        }
    }
}
