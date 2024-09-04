using AuthServer.Data.Requests;
using AuthServer.Data.Responses;
using MediatR;

namespace AuthServer.Features.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<UpdateUserResponse>
{
    public UpdateUserInput Payload { get; init; }
}
