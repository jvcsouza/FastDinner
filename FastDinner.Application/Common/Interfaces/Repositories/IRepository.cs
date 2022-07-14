namespace FastDinner.Application.Common.Interfaces.Repositories;

public interface IRepository<T>
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
}