using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.Models.Dtos;
using FoodService.Data.Models;
using MediatR;

namespace FoodService.Data.Responses;

public class SearchFoodsByNameResponse : BaseResponse
{
    public List<Food> Data { get; set; } = new();
    public PagingDto Paging { get; set; } = new();
}
