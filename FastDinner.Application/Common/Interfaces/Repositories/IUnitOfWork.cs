namespace FastDinner.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<T> Add<T>(T entity) where T : class;
    Task ExecuteTransaction(Action action);
    Task ExecuteTransaction(Func<Task> task);
    Task<T> ExecuteTransaction<T>(Func<Task<T>> task);
    Task<int> CommitAsync();

    void ExecuteAfterSave(Func<object[], Task> func, params object[] args);
}