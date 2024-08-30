using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Data.Context;
using OrderService.Repositories.Implements;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories;

public class UnitOfRepository : IUnitOfRepository
{
    private readonly OrderDbContext _context;
    private IDbContextTransaction _transaction;
    public IFoodRepository Food { get; set; }
    public IOrderRepository Order { get; set; }
    public IUserRepository User { get; set; }
    public IOrderDetailRepository OrderDetail { get; set; }

    public UnitOfRepository(OrderDbContext context)
    {
        _context = context;
        Food = new FoodRepository(context);
        Order = new OrderRepository(context);
        OrderDetail = new OrderDetailRepository(context);
        User = new UserRepository(context);
    }
    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> OpenTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
        return _transaction;
    }
    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
