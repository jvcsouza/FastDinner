using FastDinner.Application.Common.Interfaces.Repositories;

namespace FastDinner.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DinnerContext _context;
    private readonly Queue<Event> _events = new();

    public UnitOfWork(DinnerContext context)
    {
        _context = context;
    }

    public void ExecuteAfterSave(Func<object[], Task> func, params object[] args)
    {
        _events.Enqueue(new Event(func, args));
    }

    public async Task<T> Add<T>(T entity) where T : class
    {
        return (await _context.Set<T>().AddAsync(entity)).Entity;
    }

    public async Task ExecuteTransaction(Action action)
    {
        await ExecuteTransaction(() => new Task(action));
    }

    public async Task ExecuteTransaction(Func<Task> task)
    {
        await using var db = await _context.Database.BeginTransactionAsync();
        try
        {
            await task();
            await CommitAsync();
            await db.CommitAsync();
        }
        catch (Exception)
        {
            await db.RollbackAsync();
            throw;
        }
    }

    public async Task<T> ExecuteTransaction<T>(Func<Task<T>> task)
    {
        await using var db = await _context.Database.BeginTransactionAsync();
        try
        {
            var resp = await task();
            await CommitAsync();
            await db.CommitAsync();
            return resp;
        }
        catch (Exception)
        {
            await db.RollbackAsync();
            throw;
        }
    }

    public async Task<int> CommitAsync()
    {
        if (!_events.Any()) return await _context.SaveChangesAsync();

        var actualEvents = new Queue<Event>(_events);
        
        _events.Clear();

        await _context.SaveChangesAsync();

        while (actualEvents.TryDequeue(out var @event))
        {
            await @event.ActionOb(@event.Parameters);
        }

        return await CommitAsync();
    }

    // public async Task RollbackAsync()
    // {
    //     await _context.Database.RollbackTransactionAsync();
    // }
}

public class Event
{
    public Event(Func<object[], Task> action, object[] parameters)
    {
        ActionOb = action;
        Parameters = parameters;
    }

    public Func<object[], Task> ActionOb { get; }

    public object[] Parameters { get; }
}