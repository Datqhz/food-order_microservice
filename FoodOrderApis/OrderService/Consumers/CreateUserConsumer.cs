using System.Text.Json;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.UserCommands.CreateUser;

namespace OrderService.Consumers;

public class CreateUserConsumer : IConsumer<CreateUserInfo>
{
    private readonly IMediator _mediator;
    private readonly ILogger<CreateUserConsumer> _logger;

    public CreateUserConsumer(IMediator mediator, ILogger<CreateUserConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<CreateUserInfo> context)
    {
        var functionName = $"{nameof(CreateUserConsumer)} - {nameof(Consume)} =>";
        try
        {
            var message = context.Message;
            _logger.LogInformation($"{functionName} Message = {JsonSerializer.Serialize(message)}");
            await _mediator.Send(new CreateUserCommand
            {
                Payload = new ModifyUserInput
                {
                    UserId = message.UserId,
                    DisplayName = message.DisplayName,
                    PhoneNumber = message.PhoneNumber
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} Has error: {ex.Message}");
        }
        
    }
}
