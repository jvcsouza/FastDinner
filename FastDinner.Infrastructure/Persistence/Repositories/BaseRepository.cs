using System.Collections;
using FastDinner.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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
        var modifyEntity = Context.Set<T>().Update(entity).Entity;

        var enumerableType = typeof(IEnumerable);
        var collectionType = typeof(ICollection);
        var stringType = typeof(string);
        var byteType = typeof(byte[]);
        var notMapped = typeof(NotMappedAttribute);

        var methodSet = Context.GetType()
            .GetMethod(nameof(Context.Set), new Type[] { });

        entity.GetType().GetProperties()
            .Where(p => (enumerableType.IsAssignableFrom(p.PropertyType) || collectionType.IsAssignableFrom(p.PropertyType)) && !stringType.IsAssignableFrom(p.PropertyType) && !byteType.IsAssignableFrom(p.PropertyType)
                        && !p.CustomAttributes.Any(_ => Attribute.IsDefined(p, notMapped))).ToList()
            .ForEach(p =>
            {
                Console.WriteLine(p.Name);

                var itens = (dynamic)p.GetGetMethod()!.Invoke(entity, new object[] { });

                //Console.WriteLine(JsonConvert.SerializeObject(itens, Formatting.Indented));

                var setResult = ((dynamic)methodSet!.MakeGenericMethod(p.PropertyType.GetGenericArguments().First())
                        .Invoke(Context, new object[] { }));

                foreach (var it in itens ?? new List<object>())
                    Console.WriteLine(it.Name + ": " + Context.Entry(it).State);

                setResult!.AttachRange(itens);

                foreach (var it in itens)
                {
                    //if (Context.Entry(it).State == EntityState.Modified)
                    //    setResult!.Add(it);

                    try
                    {
                        Console.WriteLine(it.Name + ": " + Context.Entry(it).State);
                    }
                    catch
                    {
                        //
                    }
                }

                //if (p.PropertyType.GetGenericArguments().Any())
                //{
                //    ((dynamic)methodSet.MakeGenericMethod(p.PropertyType.GetGenericArguments().First())
                //        .Invoke(this, new object[] { }));


                //    dependents.Add(
                //        new KeyValuePair<string, Type>(p.Name, ));
                //}
            });

        return Task.FromResult(modifyEntity);
    }
}