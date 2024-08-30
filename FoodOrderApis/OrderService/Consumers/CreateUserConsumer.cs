using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.UserCommands.CreateUser;

namespace OrderService.Consumers;

public class CreateUserConsumer : IConsumer<CreateUserInfo>
{
    private readonly IMediator _mediator;

    public CreateUserConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<CreateUserInfo> context)
    {
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
