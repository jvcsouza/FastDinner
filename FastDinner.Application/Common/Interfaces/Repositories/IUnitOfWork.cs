namespace FastDinner.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task ExecuteTransaction(Action action);
    Task ExecuteTransaction(Func<Task> task);
    Task<int> CommitAsync();

    void AddEventAfterSave(Func<object[], Task> func, params object[] args);
}