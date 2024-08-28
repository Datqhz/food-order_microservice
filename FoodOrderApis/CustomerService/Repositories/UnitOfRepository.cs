using CustomerService.Data.Context;
using CustomerService.Repositories.Implements;
using CustomerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerService.Repositories;

public class UnitOfRepository : IUnitOfRepository
{
    private readonly CustomerDbContext _context;
    private IDbContextTransaction _transaction;
    public IUserInfoRepository User { get; private set; }

    public UnitOfRepository(CustomerDbContext customerDbContext)
    {
        _context = customerDbContext;
        User = new UserInfoRepository(_context);
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