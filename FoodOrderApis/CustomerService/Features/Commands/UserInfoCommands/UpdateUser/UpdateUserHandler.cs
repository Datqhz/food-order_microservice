using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CustomerService.Features.Commands.UserInfoCommands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ISendEndpointCustomProvider _sendEndpoint;
    private readonly ICustomHttpContextAccessor _httpContextAccessor;
    public UpdateUserHandler(IUnitOfRepository unitOfRepository, ISendEndpointCustomProvider sendEndpoint, ICustomHttpContextAccessor httpContextAccessor)
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<UpdateUserInfoResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateUserInfoResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            UpdateUserValidator validator = new UpdateUserValidator();
            var validationResult = validator.Validate(request);
            var payload = request.Payload;
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
                return response;
            }

            var currentUserId = _httpContextAccessor.GetCurrentUserId();
            if (currentUserId != payload.Id)
            {
                response.StatusCode = (int)ResponseStatusCode.Forbidden;
                response.StatusText = $"You don't have permission to update this user";
                return response;
            }
            var user = await _unitOfRepository.User.GetById(payload.Id);
            if (user == null)
            {
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "User not found";
                return response;
            }
            user.DisplayName = payload.DisplayName;
            user.PhoneNumber = payload.PhoneNumber;
            user.IsActive = payload.IsActive;
            bool updateResult = _unitOfRepository.User.Update(user);
            await _unitOfRepository.CompleteAsync();
            if (updateResult)
            {
                await _sendEndpoint.SendMessage<UpdateUserInfo>(new UpdateUserInfo
                {
                    UserId = user.Id,
                    DisplayName = user.DisplayName,
                    IsActive = user.IsActive,
                    PhoneNumber = user.PhoneNumber
                }, cancellationToken, null);
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "User updated";
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.InternalServerError;
                response.StatusText = "Internal Server Error";
                response.ErrorMessage = "Something wrong happened";
            }
            return response;
            
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
