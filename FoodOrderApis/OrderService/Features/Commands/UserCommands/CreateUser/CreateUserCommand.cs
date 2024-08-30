
using MediatR;
using OrderService.Data.Requests;

namespace OrderService.Features.Commands.UserCommands.CreateUser;

public class CreateUserCommand : IRequest
{
    public ModifyUserInput Payload { get; set; }
}
