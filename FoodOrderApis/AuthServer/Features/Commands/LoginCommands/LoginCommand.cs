using AuthServer.Data.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Features.Commands.LoginCommands;

public class LoginCommand : IRequest<ObjectResult>
{
    public LoginRequest Payload { get; set; }
}
