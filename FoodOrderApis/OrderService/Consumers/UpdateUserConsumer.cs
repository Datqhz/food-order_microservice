using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using MediatR;
using OrderService.Data.Requests;
using OrderService.Features.Commands.UserCommands.UpdateUser;

namespace OrderService.Consumers;

public class UpdateUserConsumer : IConsumer<UpdateUserInfo>
{
    private readonly IMediator _mediator;

    public UpdateUserConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<UpdateUserInfo> context)
    {
        var message = context.Message;
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
}
