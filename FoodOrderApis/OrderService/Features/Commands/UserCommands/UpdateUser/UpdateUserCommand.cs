using MediatR;
using OrderService.Data.Requests;

namespace OrderService.Features.Commands.UserCommands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public ModifyUserRequest Payload { get; set; }
}
