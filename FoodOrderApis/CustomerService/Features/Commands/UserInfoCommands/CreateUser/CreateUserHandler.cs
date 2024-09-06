using CustomerService.Data.Models;
using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(IUnitOfRepository unitOfRepository, ILogger<CreateUserHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }

    public async Task<CreateUserInfoResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(CreateUserHandler);
        _logger.LogInformation($"{functionName} - Start");
        var response = new CreateUserInfoResponse() { StatusCode = (int)ResponseStatusCode.BadRequest };
        try
        {
            var createCustomerValidator = new CreateUserValidator();
            var validationResult = createCustomerValidator.Validate(request);
            var payload = request.Payload;
            if (!validationResult.IsValid)
            {
                _logger.LogInformation($"{functionName} => Invalid request : Message = {validationResult.ToString("-")}");
                response.StatusText = validationResult.ToString("~");
                return response;
            }
            else
            {
                await _unitOfRepository.User.Add(new UserInfo
                {
                    Id = payload.UserId,
                    Role = payload.Role,
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
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} => Error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}