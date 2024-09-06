using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string UserId { get; set; }
}
