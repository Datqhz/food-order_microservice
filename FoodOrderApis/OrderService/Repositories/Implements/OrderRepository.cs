using OrderService.Data.Context;
using OrderService.Data.Models;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories.Implements;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
    }
}
