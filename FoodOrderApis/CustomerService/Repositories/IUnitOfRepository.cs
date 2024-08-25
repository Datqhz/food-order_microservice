using CustomerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerService.Repositories;

public interface IUnitOfRepository
{
    ICustomerRepository Customer { get; }
    Task CompleteAsync();
    Task<IDbContextTransaction> OpenTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    void Dispose();
}