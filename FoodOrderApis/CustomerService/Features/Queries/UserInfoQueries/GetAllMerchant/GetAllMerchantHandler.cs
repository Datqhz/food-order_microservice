using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Constants;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Features.Queries.UserInfoQueries.GetAllMerchant;

public class GetAllMerchantHandler : IRequestHandler<GetAllMerchantQuery, GetAllMerchantByIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetAllMerchantHandler> _logger;

    public GetAllMerchantHandler(IUnitOfRepository unitOfRepository, ILogger<GetAllMerchantHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetAllMerchantByIdResponse> Handle(GetAllMerchantQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetAllMerchantQuery);
        _logger.LogInformation($"{functionName} - Start");
        var response = new GetAllMerchantByIdResponse(){StatusCode = (int)ResponseStatusCode.InternalServerError};

        try
        {
            var merchants = await _unitOfRepository.User.Where(u => u.Role == Constants.Role.Merchant).AsNoTracking()
                .ToListAsync(cancellationToken);
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.Data = merchants;
            response.StatusText = $"Successfully retrieved all merchants";
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} - Error: {ex.Message}");
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
