using FastDinner.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly DinnerContext Context;

    protected BaseRepository(DinnerContext context)
    {
        Context = context;
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public virtual Task<T> UpdateAsync(T entity)
    {
        return Task.FromResult(Context.Set<T>().Update(entity).Entity);
    }
}