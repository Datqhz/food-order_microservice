using System.Linq.Expressions;

namespace CustomerService.Repositories.Interfaces;

public interface IGenericRepository<T>
{
    IQueryable<T> GetAll();
    Task<T> GetById(object id);
    Task<T> Add(T entity);
    bool Update(T entity);
    bool Delete(T entity);
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    
}