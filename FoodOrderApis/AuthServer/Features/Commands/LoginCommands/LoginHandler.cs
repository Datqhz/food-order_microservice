using System.Net;
using AuthServer.Data.Dtos;
using AuthServer.Data.Dtos.Responses;
using AuthServer.Data.Models;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using IdentityModel.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Commands.LoginCommands;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfRepository _unitOfRepository;

    public LoginHandler(IUnitOfRepository unitOfRepository, IPublishEndpoint publishEndpoint)
    {
        _unitOfRepository = unitOfRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginResponse = new LoginResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new LoginValidator();
            var validateResult = validator.Validate(request);
            if (!validateResult.IsValid)
            {
                loginResponse.StatusText = "Bad Request";
                loginResponse.ErrorMessage = validateResult.ToString("~");
                return loginResponse;
            }

            var payload = request.Payload;
            var user = await _unitOfRepository.User.Where(u => u.UserName == payload.Username)
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                loginResponse.StatusText = "Bad Request";
                loginResponse.ErrorMessage = "Invalid information";
                return loginResponse;
            }
            var client = await _unitOfRepository.Client.Where(cl => cl.Id == user.ClientId)
                .Select(_ => new Client
                {
                    Id = _.Id,
                    ClientId = _.ClientId
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (client == null)
            {
                loginResponse.StatusText = "Bad Request";
                loginResponse.ErrorMessage = "Invalid information";
                return loginResponse;
            }
            var clientSecret = await _unitOfRepository.ClientSecret.Where(cs => cs.ClientId == client.Id)
                .Select(_ => _.SecretName).FirstOrDefaultAsync(cancellationToken);
            var scopes = payload.Scope.Split(' ');
            var clientScopes = await _unitOfRepository.ClientScope.Where(sc => sc.ClientId == client.Id)
                .Select(cs => cs.Scope).ToListAsync();
            foreach (var scope in scopes)
                if (clientScopes.All(cs => cs.ToLower() != scope.ToLower()))
                {
                    loginResponse.StatusText = "Bad Request";
                    loginResponse.ErrorMessage = "Invalid information";
                    return loginResponse;
                }
            var httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5092");
            if (discovery.IsError)
            {
                loginResponse.StatusCode = (int)ResponseStatusCode.InternalServerError;
                loginResponse.StatusText = "Internal server error";
                loginResponse.ErrorMessage = discovery.Error;
                return loginResponse;
            }

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
                loginResponse.StatusText = "OK";
                loginResponse.StatusCode = (int)ResponseStatusCode.OK;
                loginResponse.Data = new LoginDto
                {
                    AccessToken = response.AccessToken,
                    Scope = response.Scope,
                    Expired = response.ExpiresIn
                };
            }
            else
            {
                loginResponse.StatusCode = (int)ResponseStatusCode.InternalServerError;
                loginResponse.StatusText = "Internal server error";
                loginResponse.ErrorMessage = response.ErrorDescription;
            }
            return loginResponse;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            loginResponse.StatusCode = (int)ResponseStatusCode.InternalServerError;
            loginResponse.StatusText = "Internal server error";
            loginResponse.ErrorMessage = ex.Message;
            return loginResponse;
        }
    }
}

