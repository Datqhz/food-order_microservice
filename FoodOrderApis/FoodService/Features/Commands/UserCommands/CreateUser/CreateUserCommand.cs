using FoodService.Data.Requests;
using MediatR;

namespace FoodService.Features.Commands.UserCommands.CreateUser;

public class CreateUserCommand : IRequest
{
    public ModifyUserRequest Payload { get; set; }
}
