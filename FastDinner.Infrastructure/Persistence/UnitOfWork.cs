using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Common;
using FastDinner.Domain.Model;
using System.Linq;

namespace FastDinner.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DinnerContext _context;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly Queue<Event> _events = new();

    public UnitOfWork(DinnerContext context, IDomainEventDispatcher dispatcher)
    {
        _context = context;
        _dispatcher = dispatcher;
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
        await ExecuteTransaction(() => Task.Run(action));
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
        var result = await _context.SaveChangesAsync();

        var domainEntities = _context.ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in domainEntities)
        {
            entity.ClearDomainEvents();
        }

        if (domainEvents.Any())
        {
            await _dispatcher.DispatchAsync(domainEvents);
        }

        if (_events.Any())
        {
            var actualEvents = new Queue<Event>(_events);
            _events.Clear();

            while (actualEvents.TryDequeue(out var @event))
            {
                await @event.ActionOb(@event.Parameters);
            }

            return await CommitAsync();
        }

        return result;
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