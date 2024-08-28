using FoodOrderApis.Common.MassTransit;
using FoodService.Data.Requests;
using FoodService.Features.Commands.UserCommands.CreateUser;
using MassTransit;
using MediatR;

namespace FoodService.Consumers;

public class CreateUserConsumer : IConsumer<CreateUserInfo>
{
    private readonly IMediator _mediator;

    public CreateUserConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<CreateUserInfo> context)
    {
        Console.WriteLine("CreateUserConsumer");
        var message = context.Message;
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
}
