﻿using FoodService.Data.Requests;
using MediatR;

namespace FoodService.Features.Commands.UserCommands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public ModifyUserRequest Payload { get; set; }
}
