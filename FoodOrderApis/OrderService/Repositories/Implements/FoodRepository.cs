using OrderService.Data.Context;
using OrderService.Data.Models;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories.Implements;

public class FoodRepository : GenericRepository<Food>, IFoodRepository
{
    public FoodRepository(OrderDbContext context) : base(context)
    {
    }
}
