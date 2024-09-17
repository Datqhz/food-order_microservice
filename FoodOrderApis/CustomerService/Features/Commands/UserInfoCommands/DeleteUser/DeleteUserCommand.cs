using CustomerService.Data.Responses;
using FoodOrderApis.Common.Helpers;
using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.DeleteUser;

public class DeleteUserCommand : IRequest<DeleteUserInfoResponse>
{
    public string UserId { get; set; }
}
