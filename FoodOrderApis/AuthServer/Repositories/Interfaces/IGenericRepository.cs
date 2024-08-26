namespace AuthServer.Repositories.Interfaces;

public interface IGenericRepository<T>
{
    IQueryable<T> GetAll();
    Task<T> GetById(int id);
    Task <T> Add(T entity); 
    bool Update(T entity);
    bool Delete(T entity);
    
}
