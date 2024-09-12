using AuthServer.Data.Requests;
using AuthServer.Data.Dtos.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Features.Commands.Register;

public class RegisterCommand : IRequest<RegisterResponse>
{
    public RegisterRequest Payload { get; set; }
}
