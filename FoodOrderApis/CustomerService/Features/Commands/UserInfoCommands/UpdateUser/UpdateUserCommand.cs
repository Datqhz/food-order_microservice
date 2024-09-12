﻿using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.UpdateUser;

public class UpdateUserCommand : IRequest<UpdateUserInfoResponse>
{
    public UpdateUserInfoRequest Payload { get; set; }
}
