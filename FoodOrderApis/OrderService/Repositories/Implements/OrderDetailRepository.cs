using OrderService.Data.Context;
using OrderService.Data.Models;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories.Implements;

public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(OrderDbContext context) : base(context)
    {
    }
}
