using FoodOrderApis.Common.MassTransit.Consumers;
using MediatR;

namespace CustomerService.Features.Commands.AccountCommands.CreateAccountCommand;

public class CreateAccountRequest : IRequest
{
    public CreateAccount Data { get; set; }
}
