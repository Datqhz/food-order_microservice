using System.Net;
using System.Security.Claims;
using AuthServer.Data.Models;
using AuthServer.Data.Responses;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Features.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly UserManager<User> _userManager;
    private readonly ClaimsPrincipal _user;

    public UpdateUserHandler(IUnitOfRepository unitOfRepository, UserManager<User> userManager, ClaimsPrincipal user)
    {
        _unitOfRepository = unitOfRepository;
        _userManager = userManager;
        _user = user;
    }
    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        var response = new UpdateUserResponse(){StatusCode = (int)HttpStatusCode.BadRequest};
        try
        {
            var validator = new UpdateUserValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("-");
                return response;
            }
            var userIdRequest = _user.FindFirst("sub")?.Value;
            if (userIdRequest != payload.UserId)
            {
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.StatusText = "You don't have permission to update this user";
                return response;
            }
            var user = await _unitOfRepository.User.GetById(payload.UserId);
            if (user == null)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusText = "User not found";
                return response;
            }
            else
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, payload.OldPassword);
                if (!checkPassword)
                {
                    response.StatusCode = (int)ResponseStatusCode.BadRequest;
                    response.StatusText = "Old password incorrect";
                    return response;
                }
                var newHashedPassword = new PasswordHasher<User>().HashPassword(user, payload.NewPassword);
                user.PasswordHash = newHashedPassword;
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.StatusText = "User updated";
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusText = "Can't update user";
                }
                return response;
            }
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
