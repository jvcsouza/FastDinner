using FastDinner.Application.Common.Interfaces.Repositories;

namespace FastDinner.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DinnerContext _context;
    private readonly Queue<Events> _events = new();

    public UnitOfWork(DinnerContext context)
    {
        _context = context;
    }

    public void AddEventAfterSave(Func<object[], Task> func, params object[] args)
    {
        _events.Enqueue(new Events(func, args));
    }

    public async Task ExecuteTransaction(Action action)
    {
        await ExecuteTransaction(() => Task.FromResult(action));
    }

    public async Task ExecuteTransaction(Func<Task> task)
    {
        await using var db = await _context.Database.BeginTransactionAsync();
        try
        {
            await task();
            await db.CommitAsync();
        }
        catch (Exception)
        {
            await db.RollbackAsync();
            throw;
        }
    }

    public async Task<int> CommitAsync()
    {
        while (_events.TryDequeue(out var @event))
        {
            await @event.ActionOb(@event.Parameters);
        }

        return await _context.SaveChangesAsync();
    }

    // public async Task RollbackAsync()
    // {
    //     await _context.Database.RollbackTransactionAsync();
    // }
}

public class Events
{
    public Events(Func<object[], Task> action, object[] parameters)
    {
        ActionOb = action;
        Parameters = parameters;
    }

    public Func<object[], Task> ActionOb { get; private set; }

    public object[] Parameters { get; private set; }
}