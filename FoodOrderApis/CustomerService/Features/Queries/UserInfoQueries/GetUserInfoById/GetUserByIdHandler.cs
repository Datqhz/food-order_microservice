using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;

namespace CustomerService.Features.Queries.UserInfoQueries.GetUserInfoById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserInfoByIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetUserByIdHandler> _logger;

    public GetUserByIdHandler(IUnitOfRepository unitOfRepository, ILogger<GetUserByIdHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetUserInfoByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetUserByIdHandler);
        _logger.LogInformation($"{functionName} - Start");
        var response = new GetUserInfoByIdResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var user = await _unitOfRepository.User.GetById(request.UserId);
            if (user == null)
            {
                _logger.LogError($"{functionName} - User Not Found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "User does not exist";
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "User retrieved";
                response.Data = user;
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} - Error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
