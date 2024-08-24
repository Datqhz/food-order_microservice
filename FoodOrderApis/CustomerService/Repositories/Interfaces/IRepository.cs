namespace CustomerService.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Add(T entity);
    Task<bool> Update(T entity);
    Task<bool> Delete(int id);
    Task SaveChanges();
}