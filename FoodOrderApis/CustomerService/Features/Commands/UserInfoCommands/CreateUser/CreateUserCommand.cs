using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Features.Commands.UserInfoCommands.CreateUserInfo;

public class CreateUserCommand : IRequest<CreateUserInfoResponse>
{
    public CreateUserInfoInput Payload { get; set; }
}