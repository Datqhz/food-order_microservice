﻿using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Features.Commands.UserInfoCommands.CreateUser;

public class CreateUserCommand : IRequest<CreateUserInfoResponse>
{
    public CreateUserInfoRequest Payload { get; set; }
}