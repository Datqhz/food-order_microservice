using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<DeleteUserHandler> _logger;

    public DeleteUserHandler(IUnitOfRepository unitOfRepository, ILogger<DeleteUserHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<DeleteUserInfoResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(DeleteUserCommand);
        var userId = request.UserId;
        var response = new DeleteUserInfoResponse(){StatusCode = (int)ResponseStatusCode.OK};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var user = await _unitOfRepository.User.GetById(userId);
            if (user == null)
            {
                _logger.LogError($"{functionName} => User not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                return response;
            }
            user.IsActive = false;
            var deleteResult = _unitOfRepository.User.Update(user);
            if (!deleteResult)
            {
                throw new Exception("Can't delete user");
            }
            await _unitOfRepository.CompleteAsync();
            
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            _logger.LogError(ex, $"{functionName} => Has error: Message = {ex.Message}");
            return response;
        }
    }
}
