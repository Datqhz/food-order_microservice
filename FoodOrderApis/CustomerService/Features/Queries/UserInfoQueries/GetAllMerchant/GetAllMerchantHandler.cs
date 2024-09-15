using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Constants;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.Models.Dtos;
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
            var payload = request.Payload;
            var merchants = _unitOfRepository.User
                .Where(u => 
                    u.Role == Constants.Role.Merchant &&
                       u.IsActive == true).AsNoTracking();
            int totalItems = await merchants.CountAsync(cancellationToken);
            if (payload != null)
            {
                response.Data = await merchants.Skip((payload.PageNumber - 1) * payload.MaxPerPage)
                    .Take(payload.MaxPerPage)
                    .ToListAsync(cancellationToken);
                response.Paging = new PagingDto
                {
                    TotalItems = totalItems,
                    PageNumber = payload.PageNumber,
                    TotalPages = (int)Math.Ceiling((double)totalItems / (double)payload.MaxPerPage),
                    MaxPerPage = payload.MaxPerPage
                };
            }
            else
            {
                response.Data = await merchants.ToListAsync(cancellationToken);
            }
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = $"Successfully retrieved all merchants";
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} - Error: {ex.Message}");
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
