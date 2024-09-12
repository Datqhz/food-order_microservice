
using MediatR;
using OrderService.Data.Requests;

namespace OrderService.Features.Commands.UserCommands.CreateUser;

public class CreateUserCommand : IRequest
{
    public ModifyUserRequest Payload { get; set; }
}
