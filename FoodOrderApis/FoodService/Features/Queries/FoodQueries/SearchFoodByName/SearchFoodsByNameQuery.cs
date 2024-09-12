using FoodService.Data.Requests;
using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Queries.FoodQueries.SearchFoodByName;

public class SearchFoodsByNameQuery : IRequest<SearchFoodsByNameResponse>
{
    public SearchFoodsByNameRequest Payload { get; set; }
}
