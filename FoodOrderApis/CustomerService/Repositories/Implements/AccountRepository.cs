using CustomerService.Data.Context;
using CustomerService.Data.Models;
using CustomerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Repositories.Implements;

public class AccountRepository : IAccountRepository
{
    private readonly CustomerDbContext _context;
    private readonly DbSet<Account> _dbSet;

    public AccountRepository(CustomerDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Account>();
        
    }
    public Task<IEnumerable<Account>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Account> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Account> Add(Account entity)
    {
        var result = await _dbSet.AddAsync(entity);
        await SaveChanges();
        return result.Entity;
    }

    public async Task<bool> Update(Account entity)
    {
        try
        {
            _dbSet.Update(entity);
            await SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChanges()
    {   
        await _context.SaveChangesAsync();
    }
}
