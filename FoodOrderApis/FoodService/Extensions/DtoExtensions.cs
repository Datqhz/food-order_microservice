using FoodService.Data.Models;
using FoodService.Data.Models.Dtos;

namespace FoodService.Extensions;

public static class DtoExtensions
{
    public static FoodDto AsDto(this Food food)
    {
        return new FoodDto
        {
            Id = food.Id,
            Name = food.Name,
            Price = food.Price,
            ImageUrl = food.ImageUrl,
            Describe = food.Describe
        };
    }
}
