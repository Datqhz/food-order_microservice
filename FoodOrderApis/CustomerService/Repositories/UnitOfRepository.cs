using CustomerService.Data.Context;
using CustomerService.Repositories.Implements;
using CustomerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerService.Repositories;

public class UnitOfRepository : IUnitOfRepository
{
    private readonly CustomerDbContext _context;
    private IDbContextTransaction _transaction;
    public ICustomerRepository Customer { get; private set; }
    public IAccountRepository Account { get; private set; }

    public UnitOfRepository(CustomerDbContext customerDbContext)
    {
        _context = customerDbContext;
        Customer = new CustomerRepository(_context);
        Account  = new AccountRepository(_context);
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