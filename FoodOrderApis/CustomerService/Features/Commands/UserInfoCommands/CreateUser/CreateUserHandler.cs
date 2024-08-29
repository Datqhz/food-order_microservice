using CustomerService.Data.Models;
using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CreateUserHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }

    public async Task<CreateUserInfoResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateUserInfoResponse() { StatusCode = (int)ResponseStatusCode.BadRequest };
        try
        {
            var createCustomerValidator = new CreateUserValidator();
            var validationResult = createCustomerValidator.Validate(request);
            var payload = request.Payload;
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
            }
            else
            {
                await _unitOfRepository.User.Add(new UserInfo
                {
                    Id = payload.UserId,
                    ClientId = payload.ClientId,
                    DisplayName = payload.DisplayName,
                    CreatedDate = payload.CreatedDate,
                    PhoneNumber = payload.PhoneNumber,
                    UserName = payload.UserName,
                    IsActive = payload.IsActive,
                });
                await _unitOfRepository.CompleteAsync();
                response.StatusCode = (int)ResponseStatusCode.Created;
                response.StatusText = "Create user successfully";
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