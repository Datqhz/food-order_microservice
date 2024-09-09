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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(IUnitOfRepository unitOfRepository, UserManager<User> userManager, ILogger<LoginHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(LoginHandler);
        _logger.LogInformation($"{functionName} => ");
        var loginResponse = new LoginResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var payload = request.Payload;
            var user = await _unitOfRepository.User.Where(u => u.UserName == payload.Username)
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                _logger.LogError($"{functionName} => User not found");
                loginResponse.StatusCode = (int)ResponseStatusCode.NotFound;
                loginResponse.StatusText = "Not found";
                return loginResponse;
            }
            if (!user.IsActive)
            {
                _logger.LogError($"{functionName} => User is deleted");
                loginResponse.StatusText = "User is deleted";
                return loginResponse;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var client = await _unitOfRepository.Client.Where(cl => cl.Id == user.ClientId)
                .Select(_ => new Client
                {
                    Id = _.Id,
                    ClientId = _.ClientId
                })
                .FirstOrDefaultAsync(cancellationToken);
            var clientSecret = await _unitOfRepository.ClientSecret.Where(cs => cs.ClientId == client.Id)
                .Select(_ => _.SecretName).FirstOrDefaultAsync(cancellationToken);
            var scopes = await _unitOfRepository.RolePermission.Where(p => p.Role == roles[0])
                .Select(_ => _.Permission).ToListAsync();
            var requestScopes = "";
            foreach (var scope in scopes)
            {
                requestScopes += " " + scope;
            }
            
            var httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5092");
            if (discovery.IsError)
            {
                _logger.LogError($"{functionName} => Can't get discovery details : Message = {discovery.Error}");
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
                Scope = requestScopes.Trim(),
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
                _logger.LogError($"{functionName} => Can't get access token : Message = {response.ErrorDescription}");
                loginResponse.StatusCode = (int)response.HttpStatusCode;
                loginResponse.StatusText = "Error";
                loginResponse.ErrorMessage = response.ErrorDescription;
            }
            _logger.LogInformation($"{functionName} - End");
            return loginResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Error : Message = {ex.Message}");
            loginResponse.StatusCode = (int)ResponseStatusCode.InternalServerError;
            loginResponse.StatusText = "Internal server error";
            loginResponse.ErrorMessage = ex.Message;
            return loginResponse;
        }
    }
}

