using System.Linq.Expressions;
using FoodService.Data.Context;
using FoodService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Repositories.Implements;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly FoodDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(FoodDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public IQueryable<T> GetAll()
    {
        return _dbSet;
    }

    public async Task<T> GetById(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> Add(T entity)
    {
        var result = await _dbSet.AddAsync(entity);
        return result.Entity;
    }

    public async Task<bool> AddRange(IEnumerable<T> entities)
    {
        try
        {
            _dbSet.AddRange(entities);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Update(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Delete(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
    public virtual IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
}
