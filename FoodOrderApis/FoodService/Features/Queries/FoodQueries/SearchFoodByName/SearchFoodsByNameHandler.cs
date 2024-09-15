using FoodOrderApis.Common.Enums;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.Models.Dtos;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Features.Queries.FoodQueries.SearchFoodByName;

public class SearchFoodsByNameHandler : IRequestHandler<SearchFoodsByNameQuery, SearchFoodsByNameResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<SearchFoodsByNameHandler> _logger;

    public SearchFoodsByNameHandler(IUnitOfRepository unitOfRepository, ILogger<SearchFoodsByNameHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    
    public async Task<SearchFoodsByNameResponse> Handle(SearchFoodsByNameQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(SearchFoodsByNameHandler);
        _logger.LogInformation($"{functionName} - Start");
        var response = new SearchFoodsByNameResponse(){StatusCode = (int)ResponseStatusCode.OK};
        try
        {
            var payload = request.Payload;
            var foods = _unitOfRepository.Food.Where(f => EF.Functions.ILike(f.Name, $"%{payload.Keyword}%")).AsNoTracking();
            var totalItems = await foods.CountAsync(cancellationToken);
            if (totalItems == 0)
            {
                return response;
            }

            if (payload.SortBy == (int)SortOption.ByAlphabeticalDescending)
            {
                foods = foods.OrderByDescending(f => f.Name);
            }
            else
            {
                foods = foods.OrderBy(f => f.Name);
            }
            if (payload.MaxPerPage != null && payload.Page != null)
            {
                response.Data = await foods
                    .Skip((int)((payload.Page - 1) * payload.MaxPerPage))
                    .Take((int)payload.MaxPerPage)
                    .ToListAsync(cancellationToken);
                response.Paging = new PagingDto
                {
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / (double)payload.MaxPerPage),
                    PageNumber = (int)payload.Page,
                    MaxPerPage = (int)payload.MaxPerPage
                };
            }
            else
            {
                response.Data = await foods.ToListAsync(cancellationToken);
            }
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
