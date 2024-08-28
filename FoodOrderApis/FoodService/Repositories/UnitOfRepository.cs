using FoodService.Data.Context;
using FoodService.Repositories.Implements;
using FoodService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace FoodService.Repositories;

public class UnitOfRepository : IUnitOfRepository
{
    public IFoodRepository Food { get; set; }
    public IUserRepository User { get; set; }
    private readonly FoodDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfRepository(FoodDbContext context)
    {
        _context = context;
        Food = new FoodRepository(_context);
        User = new UserRepository(_context);
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
