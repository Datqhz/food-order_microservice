using FoodService.Data.Context;
using FoodService.Data.Models;
using FoodService.Repositories.Interfaces;

namespace FoodService.Repositories.Implements;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(FoodDbContext context) : base(context){}
}
