using System.Text.Json;
using CustomerService.Features.Commands.UserInfoCommands.DeleteUser;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;

namespace CustomerService.Consumers;

public class DeleteUserConsumer : IConsumer<DeleteUserInfo>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteUserConsumer> _logger;

    public DeleteUserConsumer(IMediator mediator, ILogger<DeleteUserConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<DeleteUserInfo> context)
    {
        var functionName = $"{nameof(DeleteUserConsumer)} - {nameof(Consume)} =>";
        try
        {
            var message = context.Message;
            _logger.LogInformation($"{functionName} Message = {JsonSerializer.Serialize(message)}");
            await _mediator.Send(new DeleteUserCommand { UserId = message.UserId });
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} Has error : Message = {ex.Message}");
        }
    }
}
