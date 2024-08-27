using AuthServer.Data.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Features.Commands.RegisterCommands;

public class RegisterCommand : IRequest<ObjectResult>
{
    public RegisterRequest Payload { get; set; }
}
