using System.Linq.Expressions;
using AuthServer.Data.Context;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AuthDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AuthDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public IQueryable<T> GetAll()
    {
        return _dbSet;
    }

    public async Task<T> GetById(int id)
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