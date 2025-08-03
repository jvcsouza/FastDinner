namespace FastDinner.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<T> AddAsync<T>(T entity) where T : class;
    Task ExecuteTransactionAsync(Action action);
    Task ExecuteTransactionAsync(Func<Task> task);
    Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> task);
    Task<int> CommitAsync();

    void ExecuteAfterSave(Func<object[], Task> func, params object[] args);
}