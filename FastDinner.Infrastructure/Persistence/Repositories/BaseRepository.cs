using System.Collections;
using FastDinner.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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

        var EnumerableType = typeof(IEnumerable);
        var collectionType = typeof(ICollection);
        var StringType = typeof(string);
        var byteType = typeof(byte[]);
        var NotMapped = typeof(NotMappedAttribute);

        var methodSet = Context.GetType()
            .GetMethod(nameof(Context.Set), new Type[] { });

        entity.GetType().GetProperties()
            .Where(p => (EnumerableType.IsAssignableFrom(p.PropertyType) || collectionType.IsAssignableFrom(p.PropertyType)) && !StringType.IsAssignableFrom(p.PropertyType) && !byteType.IsAssignableFrom(p.PropertyType)
                        && !p.CustomAttributes.Any(a => Attribute.IsDefined(p, NotMapped))).ToList()
            .ForEach(p =>
            {
                Console.WriteLine(p.Name);

                var itens = (dynamic)p.GetGetMethod()!.Invoke(entity, new object[] { });

                //Console.WriteLine(JsonConvert.SerializeObject(itens, Formatting.Indented));

                var setResult = ((dynamic)methodSet!.MakeGenericMethod(p.PropertyType.GetGenericArguments().First())
                        .Invoke(Context, new object[] { }));

                foreach (var it in itens)
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