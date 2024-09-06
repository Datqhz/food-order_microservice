using System.Text.Json;
using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.CreateUser;
using CustomerService.Repositories;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;

namespace CustomerService.Consumers;

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
        const string functionName = $"{nameof(CreateUserInfo)} - {nameof(Consume)} =>";
        try
        {
            var newUser = context.Message;
            _logger.LogInformation($"{functionName} Message = {JsonSerializer.Serialize(newUser)}");
            await _mediator.Send(new CreateUserCommand
            {
                Payload = new CreateUserInfoInput
                {
                    UserId = newUser.UserId,
                    UserName = newUser.UserName,
                    Role = newUser.Role,
                    CreatedDate = newUser.CreatedDate,
                    IsActive = newUser.IsActive,
                    DisplayName = newUser.DisplayName,
                    PhoneNumber = newUser.PhoneNumber
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} Has error: Message{ex.Message}");
        }
        
    }
}
