using FoodService.Data.Requests;
using MediatR;

namespace FoodService.Features.Commands.UserCommands.CreateUser;

public class CreateUserCommand : IRequest
{
    public ModifyUserInput Payload { get; set; }
}
