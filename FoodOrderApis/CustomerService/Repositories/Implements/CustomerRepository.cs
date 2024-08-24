using CustomerService.Data.Context;
using CustomerService.Data.Models;
using CustomerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Repositories.Implements;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;
    private readonly DbSet<Customer> _dbSet;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Customer>();
    }
    public async Task<IEnumerable<Customer>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Customer> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<Customer> Add(Customer entity)
    {
        var result = await _dbSet.AddAsync(entity);
        await SaveChanges();
        return result.Entity;
    }

    public async Task<bool> Update(Customer entity)
    {
        try
        {
            var result = _dbSet.Update(entity);
            await SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            var result = _dbSet.Remove(entity);
            await SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}