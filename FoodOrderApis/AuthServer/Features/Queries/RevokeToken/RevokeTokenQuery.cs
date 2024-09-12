using AuthServer.Data.Requests;
using AuthServer.Data.Responses;
using MediatR;

namespace AuthServer.Features.Queries.RevokeToken;

public class RevokeTokenQuery : IRequest<RevokeTokenResponse>
{
    public RevokeTokenRequest Payload;
}
