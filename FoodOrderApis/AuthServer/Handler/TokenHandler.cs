using AuthServer.Features.Commands;
using IdentityModel.Client;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Handler;

public class TokenHandler : IRequestHandler<GetTokenRequest, ObjectResult>
{
    public async Task<ObjectResult> Handle(GetTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            HttpClient httpClient = new HttpClient(); 
            var discovery = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5092");
            if (discovery.IsError)
            {
                return new ObjectResult(
                    new
                    {
                        status = StatusCodes.Status500InternalServerError,
                        statusText = "Internal server error"
                    }){StatusCode = StatusCodes.Status500InternalServerError};
            }
                
            var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = request.Data.client_id,
                ClientSecret = request.Data.client_secret,
                Scope = request.Data.scope,
                UserName = request.Data.username,
                Password = request.Data.password

            });
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ObjectResult(new
                    {
                        status = StatusCodes.Status200OK,
                        statusText = "Get token successful",
                        data = new
                        {
                            access_token = response.AccessToken,
                            scope = response.Scope,
                            expired = response.ExpiresIn
                        }
                    })
                    { StatusCode = StatusCodes.Status200OK };
            }
            else
            {
                return new ObjectResult(new
                    {
                        status = StatusCodes.Status400BadRequest,
                        statusText = "Invalid information",
                    })
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
        catch (Exception ex)
        {
            return new ObjectResult(new
                {
                    status = StatusCodes.Status500InternalServerError,
                    statusText = "Internal server error"
                })
                { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}