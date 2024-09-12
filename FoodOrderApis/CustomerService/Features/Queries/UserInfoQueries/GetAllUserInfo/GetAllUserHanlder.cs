using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Features.Queries.UserInfoQueries.GetAllUserInfo;

public class GetAllUserHanlder : IRequestHandler<GetAllUserQuery, GetAllUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetAllUserHanlder> _logger;

    public GetAllUserHanlder(IUnitOfRepository unitOfRepository, ILogger<GetAllUserHanlder> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetAllUserInfoResponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetAllUserHanlder);
        _logger.LogInformation($"{functionName} - Start");
        var response = new GetAllUserInfoResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var users = await _unitOfRepository.User.GetAll().AsNoTracking().ToListAsync();
            response.Data = users;
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get all user successfully";
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} - Error: {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
