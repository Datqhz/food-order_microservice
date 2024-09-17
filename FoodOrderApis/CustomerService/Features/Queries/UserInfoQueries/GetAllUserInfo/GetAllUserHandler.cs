using CustomerService.Data.Models;
using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Constants;
using FoodOrderApis.Common.Enums;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Features.Queries.UserInfoQueries.GetAllUserInfo;

public class GetAllUserHandler : IRequestHandler<GetAllUserQuery, GetAllUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetAllUserHandler> _logger;

    public GetAllUserHandler(IUnitOfRepository unitOfRepository, ILogger<GetAllUserHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetAllUserInfoResponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetAllUserHandler);
        _logger.LogInformation($"{functionName} - Start");
        var response = new GetAllUserInfoResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        var payload = request.Payload;
        try
        {
            var users = _unitOfRepository.User.GetAll().AsNoTracking();
            if (payload.GetBy == FilterUser.Eater)
            {
                users = users.Where(u => u.Role.ToLower() == Constants.Role.Eater.ToLower());
            }
            else
            {
                users = users.Where(u => u.Role.ToLower() == Constants.Role.Merchant.ToLower());
            }

            if (payload.SortBy == SortOption.ByAlphabeticalDescending)
            {
                users = users.OrderByDescending(u => u.DisplayName);
            }
            else
            {
                users = users.OrderBy(u => u.DisplayName);
            }
            response.Data = await users.ToListAsync(cancellationToken);
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
