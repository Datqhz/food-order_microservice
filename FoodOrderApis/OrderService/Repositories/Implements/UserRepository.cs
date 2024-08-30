using OrderService.Data.Context;
using OrderService.Data.Models;
using OrderService.Repositories.Implements;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories.Implements;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(OrderDbContext context) : base(context){}
}
