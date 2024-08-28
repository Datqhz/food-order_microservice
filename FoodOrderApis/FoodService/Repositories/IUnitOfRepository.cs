using System.Data;
using FoodService.Data.Context;
using FoodService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace FoodService.Repositories;

public interface IUnitOfRepository
{
    public IFoodRepository Food { get; set; }
    public IUserRepository User { get; set; }
    
    Task CompleteAsync();
    Task<IDbContextTransaction> OpenTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    void Dispose();
    
    
}
