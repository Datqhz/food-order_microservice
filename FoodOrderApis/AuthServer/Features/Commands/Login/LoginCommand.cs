using AuthServer.Data.Requests;
using AuthServer.Data.Dtos.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Features.Commands.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public LoginInput Payload { get; set; }
}
