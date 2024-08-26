using CustomerService.Features.Commands.AccountCommands.CreateAccountCommand;
using CustomerService.Repositories;
using FoodOrderApis.Common.MassTransit.Consumers;
using MassTransit;
using MediatR;

namespace CustomerService.Consumers;

public class CreateAccountConsumer : IConsumer<CreateAccount>
{
    private readonly IMediator _mediator;

    public CreateAccountConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<CreateAccount> context)
    {
        var newAccount = context.Message;
        await _mediator.Send(new CreateAccountRequest{Data = newAccount});
    }
}
