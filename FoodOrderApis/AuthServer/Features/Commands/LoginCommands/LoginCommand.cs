using AuthServer.Data.Requests;
using AuthServer.Data.Dtos.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Features.Commands.LoginCommands;

public class LoginCommand : IRequest<LoginResponse>
{
    public LoginRequest Payload { get; set; }
}
