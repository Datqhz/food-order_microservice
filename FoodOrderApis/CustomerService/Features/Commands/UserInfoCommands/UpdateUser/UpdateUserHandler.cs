using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit;
using MassTransit;
using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateUserHandler(IUnitOfRepository unitOfRepository, IPublishEndpoint publishEndpoint)
    {
        _unitOfRepository = unitOfRepository;
        _publishEndpoint = publishEndpoint;
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
                response.StatusText = validationResult.Errors.First().ErrorMessage;
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
                await _publishEndpoint.Publish(new UpdateUserInfo
                {
                    UserId = user.Id,
                    DisplayName = user.DisplayName,
                    IsActive = user.IsActive,
                    PhoneNumber = user.PhoneNumber
                }, cancellationToken);
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "User updated";
                return response;
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.InternalServerError;
                response.StatusText = "Internal Server Error";
                return response;
            }
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
