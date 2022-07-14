using FastDinner.Application.Common.Interfaces.Repositories;

namespace FastDinner.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DinnerContext _context;

    public UnitOfWork(DinnerContext context)
    {
        _context = context;
    }

    public async Task ExecuteTransaction(Action action)
    {
        await ExecuteTransaction(() => Task.FromResult(action));
    }

    public async Task ExecuteTransaction(Func<Task> task)
    {
        using(var db = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await task();
                db.Commit();
            }
            catch(Exception)
            {
                db.Rollback();
                throw;
            }
        }
    }

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    // public async Task RollbackAsync()
    // {
    //     await _context.Database.RollbackTransactionAsync();
    // }
}