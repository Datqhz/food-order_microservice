using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories;

public interface IUnitOfRepository
{
    public IFoodRepository Food { get; set; }
    public IOrderRepository Order { get; set; }
    public IUserRepository User { get; set; }
    public IOrderDetailRepository OrderDetail { get; set; }    
    Task CompleteAsync();
    Task<IDbContextTransaction> OpenTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    void Dispose();
}
