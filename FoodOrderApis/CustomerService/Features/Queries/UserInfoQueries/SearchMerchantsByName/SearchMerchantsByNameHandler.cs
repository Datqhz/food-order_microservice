using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Enums;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Constants = FoodOrderApis.Common.Constants.Constants;

namespace CustomerService.Features.Queries.UserInfoQueries.SearchMerchantsByName;

public class SearchMerchantsByNameHandler : IRequestHandler<SearchMerchantsByNameQuery, SearchMerchantsByNameResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<SearchMerchantsByNameHandler> _logger;

    public SearchMerchantsByNameHandler(IUnitOfRepository unitOfRepository,
        ILogger<SearchMerchantsByNameHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<SearchMerchantsByNameResponse> Handle(SearchMerchantsByNameQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(SearchMerchantsByNameHandler);
        _logger.LogInformation($"{functionName} - Start");
        var response = new SearchMerchantsByNameResponse(){StatusCode = (int)ResponseStatusCode.OK};
        try
        {
            var payload = request.Payload;
            var merchants = _unitOfRepository.User
                .Where(user => ( user.Role == Constants.Role.Merchant && EF.Functions.ILike(user.DisplayName, $"%{payload.Keyword}%"))).AsNoTracking();
            var totalItems = await merchants.CountAsync();
            if (totalItems == 0)
            {
                return response;
            }

            if (payload.SortBy == (int)SortOption.ByAlphabeticalDescending)
            {
                merchants = merchants.OrderByDescending(merchant => merchant.DisplayName);
            }
            else
            {
                merchants = merchants.OrderBy(merchant => merchant.DisplayName);
            }
            if (payload.Page != null && payload.MaxPerPage != null)
            {
                response.Data = await merchants
                    .Skip((int)((payload.Page - 1) * payload.MaxPerPage))
                    .Take((int)payload.MaxPerPage)
                    .ToListAsync(cancellationToken);
                response.Paging = new PagingDto
                {
                    TotalItems = totalItems,
                    PageNumber = (int)payload.Page,
                    TotalPages = (int)Math.Ceiling((double)totalItems / (double)payload.MaxPerPage),
                    MaxPerPage = (int)payload.MaxPerPage
                };
            }
            else
            {
                response.Data = await merchants.ToListAsync(cancellationToken);
            }
            _logger.LogInformation($"{functionName} - End");
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} => has error : Message = {ex.Message}");
            response.ErrorMessage = ex.Message;
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            return response;
        }
    }
}
