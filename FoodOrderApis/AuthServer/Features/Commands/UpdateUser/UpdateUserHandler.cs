using System.Net;
using System.Security.Claims;
using AuthServer.Data.Models;
using AuthServer.Data.Responses;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Features.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly UserManager<User> _userManager;
    private readonly ICustomHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(IUnitOfRepository unitOfRepository, UserManager<User> userManager, ICustomHttpContextAccessor httpContextAccessor, ILogger<UpdateUserHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateUserHandler);
        _logger.LogInformation($"{functionName} - Start");
        var payload = request.Payload;
        var response = new UpdateUserResponse(){StatusCode = (int)HttpStatusCode.BadRequest};
        try
        {
            var userIdRequest = _httpContextAccessor.GetCurrentUserId();
            if (userIdRequest != payload.UserId)
            {
                _logger.LogError($"{functionName} => Permission denied");
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.StatusText = "You don't have permission to update this user";
                return response;
            }
            var user = await _unitOfRepository.User.GetById(payload.UserId);
            if (user == null)
            {
                _logger.LogError($"{functionName} => User not found");
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusText = "User not found";
                return response;
            }
            else
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, payload.OldPassword);
                if (!checkPassword)
                {
                    _logger.LogError($"{functionName} => Old password incorrect");
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
                    throw new Exception("Cannot update user");
                }
                _logger.LogInformation($"{functionName} - End");
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} => Error : Message = {ex.Message}");
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
