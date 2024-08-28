using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.CreateUser;
using CustomerService.Repositories;
using FoodOrderApis.Common.MassTransit;
using MassTransit;
using MediatR;

namespace CustomerService.Consumers;

public class CreateUserConsumer : IConsumer<CreateUserInfo>
{
    private readonly IMediator _mediator;

    public CreateUserConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<CreateUserInfo> context)
    {
        var newUser = context.Message;
        await _mediator.Send(new CreateUserCommand{Payload = new CreateUserInfoInput
        {
            UserId = newUser.UserId,
            UserName = newUser.UserName,
            ClientId = newUser.ClientId,
            CreatedDate = newUser.CreatedDate,
            IsActive = newUser.IsActive,
            DisplayName = newUser.DisplayName,
            PhoneNumber = newUser.PhoneNumber
        }});
    }
}
