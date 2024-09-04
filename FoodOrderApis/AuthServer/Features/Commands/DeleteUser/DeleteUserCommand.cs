using AuthServer.Data.Responses;
using MediatR;

namespace AuthServer.Features.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<DeleteUserResponse>
{
    public string UserId { get; set; }
}
