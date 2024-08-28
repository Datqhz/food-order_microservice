using FoodService.Data.Context;
using FoodService.Data.Models;
using FoodService.Repositories.Interfaces;

namespace FoodService.Repositories.Implements;

public class FoodRepository : GenericRepository<Food>, IFoodRepository
{
    public FoodRepository(FoodDbContext context) : base(context)
    {
    }
}
