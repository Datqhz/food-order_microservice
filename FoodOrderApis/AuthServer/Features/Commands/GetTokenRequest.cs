using AuthServer.Data.Dtos.Inputs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Features.Commands;

public class GetTokenRequest : IRequest<ObjectResult>
{
    public GetTokenRequestInput Data { get; set; }
}