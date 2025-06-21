using System.Collections;
using FastDinner.Application.Common;
using FastDinner.Domain.Model;
using FastDinner.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace FastDinner.Infrastructure.Persistence;

public class DinnerContext : DbContext
{
    private readonly AppScope _appScope;

    public DinnerContext(DbContextOptions<DinnerContext> options, AppScope appScope) : base(options)
    {
        _appScope = appScope;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(x => x.Restaurant)
            .WithMany(x => x.Reservations)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        modelBuilder.Entity<Menu>()
            .Property(x => x.Ver)
            .IsRowVersion();

        modelBuilder.Entity<Product>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Employee>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Menu>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Order>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId)
            .HasOne(x => x.Table);

        modelBuilder.Entity<Order>()
            .HasMany(x => x.Sheets)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId);

        modelBuilder.Entity<OrderSheet>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Order>()
            .HasOne(x => x.Customer);

        modelBuilder.Entity<Reservation>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Table>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Customer>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new CommandInterceptor());

        base.OnConfiguring(optionsBuilder);
    }

    private void SetDefaults(CancellationToken cancellationToken)
    {
        foreach (var property in ChangeTracker.Entries<IRestaurant>()
                     .Where(w => w.State == EntityState.Added && w.Entity.RestaurantId == default))
        {
            property.Entity.RestaurantId = AppScope.Restaurant.RestaurantId;

            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        try
        {
            SetDefaults(cancellationToken);

            //var method = GetType().GetMethod("UpdateEntity");

            //var entitiesChanges = ChangeTracker.Entries()
            //    .Where(x => x.State == EntityState.Modified);

            //Console.WriteLine(JsonConvert.SerializeObject(entitiesChanges.Select(x => x.Entity.GetType().FullName), Formatting.Indented));

            //foreach (var newEntity in entitiesChanges)
            //{
            //    method!.MakeGenericMethod(newEntity.Entity.GetType())
            //        .Invoke(this, new[] { newEntity.Entity, newEntity.Entity });
            //}

            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entity = ex.Entries.Single();
            var props = await entity.GetDatabaseValuesAsync(cancellationToken);

            Console.WriteLine(JsonConvert.SerializeObject(entity.Entity.GetType(), Formatting.Indented));

            throw;
        }
    }

    private IEnumerable<KeyValuePair<string, Type>> GetDependencies<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            return Enumerable.Empty<KeyValuePair<string, Type>>();
        }

        var enumerableType = typeof(IEnumerable);
        var stringType = typeof(string);
        var notMapped = typeof(NotMappedAttribute);

        ICollection<KeyValuePair<string, Type>> dependents = new List<KeyValuePair<string, Type>>();

        entity.GetType().GetProperties()
            .Where(p => enumerableType.IsAssignableFrom(p.PropertyType) && !stringType.IsAssignableFrom(p.PropertyType) &&
                        !p.CustomAttributes.Any(_ => Attribute.IsDefined(p, notMapped)) && !p.GetGetMethod().IsVirtual).ToList()
            .ForEach(p =>
            {
                if (p.PropertyType.GetGenericArguments().Any())
                {
                    dependents.Add(
                        new KeyValuePair<string, Type>(p.Name, p.PropertyType.GetGenericArguments().First()));
                }
            });

        return dependents;
    }

    public void UpdateEntity<TEntity>(TEntity oldEntity, TEntity newEntity) where TEntity : class
    {
        var dependences = GetDependencies(oldEntity);

        Console.WriteLine(JsonConvert.SerializeObject(dependences, Formatting.Indented));

        foreach (var item in dependences)
        {
            var key = item.Value.GetProperties()
                .FirstOrDefault(p => p.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute)));

            var propertyInfo = typeof(TEntity).GetProperty(item.Key);
            if (propertyInfo != null)
            {
                var chieldList = propertyInfo.GetGetMethod();
                Func<TEntity, IEnumerable<object>> selector =
                    Delegate.CreateDelegate(typeof(Func<TEntity, IEnumerable<object>>), chieldList) as
                        Func<TEntity, IEnumerable<object>>;

                Expression<Func<object, object>> exp = u =>
                    item.Value.InvokeMember(key.Name, BindingFlags.GetProperty, null, u, null);
                var selectId = exp.Compile();

                //Filtro de todos que foram adicionados => IDs = 0
                int idToDelete = 0;
                ParameterExpression empParam = Expression.Parameter(item.Value, "emp");
                ConstantExpression equalTarget = Expression.Constant(0, idToDelete.GetType());
                if (key != null)
                {
                    BinaryExpression intEqualsId = Expression.NotEqual(Expression.PropertyOrField(empParam, key.Name), equalTarget);
                    Type delegateType = typeof(Func<,>).MakeGenericType(item.Value, typeof(bool));
                    var lambda1 = Expression.Lambda(delegateType, intEqualsId, empParam).Compile();

                    UpdateChildCollection(oldEntity, newEntity, selector, selectId, lambda1);
                }
            }
        }

        Entry(oldEntity).CurrentValues.SetValues(newEntity);
    }

    private void UpdateChildCollection<TParent, TId, TChild>(TParent dbItem, TParent newItem, Func<TParent,
        IEnumerable<TChild>> selector, Func<TChild, TId> idSelector, Delegate pred) where TChild : class
    {
        DbContext context = this;

        var dbItems = selector(dbItem).ToList();
        var newItems = selector(newItem).ToList();
        var updateItems = newItems.Where(x => (bool)pred.DynamicInvoke(x)).ToList();

        var original = dbItems.ToDictionary(idSelector);
        var updated = updateItems.ToDictionary(idSelector);
        var added = newItems.Exclude(updateItems, idSelector);

        var toRemove = original.Where(i => !updated.ContainsKey(i.Key)).ToList();
        toRemove.ForEach(i => context.Entry(i.Value).State = EntityState.Deleted);

        var toUpdate = original.Where(i => updated.ContainsKey(i.Key)).ToList();
        toUpdate.ForEach(i => context.Entry(i.Value).CurrentValues.SetValues(updated[i.Key]));

        var toAdd = added.ToList();

        var mt = context.GetType().GetMethod(nameof(context.Set));

        toAdd.ForEach(i => ((dynamic)mt!.MakeGenericMethod(i.GetType()).Invoke(context, null)!).Add(i));
    }


    public DbSet<Product> Products { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}

public static class DbExtesions
{
    public static IEnumerable<TSource> Exclude<TSource, TKey>(this IEnumerable<TSource> source,
        IEnumerable<TSource> exclude, Func<TSource, TKey> keySelector)
    {
        var excludedSet = new HashSet<TKey>(exclude.Select(keySelector));
        return source.Where(item => !excludedSet.Contains(keySelector(item)));
    }
}