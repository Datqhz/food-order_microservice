using System.Net;
using AuthServer.Data.Models;
using AuthServer.Repositories;
using IdentityModel.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Commands.LoginCommands;

public class LoginHandler : IRequestHandler<LoginCommand, ObjectResult>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfRepository _unitOfRepository;

    public LoginHandler(IUnitOfRepository unitOfRepository, IPublishEndpoint publishEndpoint)
    {
        _unitOfRepository = unitOfRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ObjectResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = new LoginValidator();
            var validateResult = validator.Validate(request);
            if (!validateResult.IsValid)
                return new ObjectResult
                (new
                    {
                        status = StatusCodes.Status400BadRequest,
                        statusText = "Bad request",
                        message = "Invalid information"
                    }
                ) { StatusCode = StatusCodes.Status400BadRequest };

            var payload = request.Payload;
            var user = await _unitOfRepository.User.Where(u => u.UserName == payload.Username)
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                return new ObjectResult
                (new
                    {
                        status = StatusCodes.Status400BadRequest,
                        statusText = "Bad request",
                        message = "Invalid information"
                    }
                ) { StatusCode = StatusCodes.Status400BadRequest };
            var client = await _unitOfRepository.Client.Where(cl => cl.Id == user.ClientId)
                .Select(_ => new Client
                {
                    Id = _.Id,
                    ClientId = _.ClientId
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (client == null)
                return new ObjectResult
                (new
                    {
                        status = StatusCodes.Status400BadRequest,
                        statusText = "Bad request",
                        message = "Invalid information"
                    }
                ) { StatusCode = StatusCodes.Status400BadRequest };
            var clientSecret = await _unitOfRepository.ClientSecret.Where(cs => cs.ClientId == client.Id)
                .Select(_ => _.SecretName).FirstOrDefaultAsync(cancellationToken);
            var scopes = payload.Scope.Split(' ');
            var clientScopes = await _unitOfRepository.ClientScope.Where(sc => sc.ClientId == client.Id)
                .Select(cs => cs.Scope).ToListAsync();
            foreach (var scope in scopes)
                if (clientScopes.All(cs => cs.ToLower() != scope.ToLower()))
                    return new ObjectResult
                    (new
                        {
                            status = StatusCodes.Status400BadRequest,
                            statusText = "Bad request",
                            message = "Invalid information"
                        }
                    ) { StatusCode = StatusCodes.Status400BadRequest };
            var httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5092");
            if (discovery.IsError)
                return new ObjectResult(
                    new
                    {
                        status = StatusCodes.Status500InternalServerError,
                        statusText = "Internal server error"
                    }) { StatusCode = StatusCodes.Status500InternalServerError };
            var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = client.ClientId,
                ClientSecret = clientSecret,
                Scope = payload.Scope,
                UserName = payload.Username,
                Password = payload.Password
            });
            if (response.HttpStatusCode == HttpStatusCode.OK)
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
                return new ObjectResult(
                    new
                    {
                        status = StatusCodes.Status500InternalServerError,
                        statusText = "Internal server error",
                        error = response.ErrorDescription
                    }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new ObjectResult(
                new
                {
                    status = StatusCodes.Status500InternalServerError,
                    statusText = "Internal server error"
                }) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}

