﻿using System.Net;
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

    public LoginHandler(IUnitOfRepository unitOfRepository, UserManager<User> userManager)
    {
        _unitOfRepository = unitOfRepository;
        _userManager = userManager;
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
            if (user.IsActive == false)
            {
                loginResponse.StatusText = "Bad Request";
                loginResponse.ErrorMessage = "This user is not active";
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

