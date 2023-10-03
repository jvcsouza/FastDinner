using FastDinner.Application.Common;
using FastDinner.Domain.Model;
using FastDinner.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence;

public class DinnerContext : DbContext
{
    public DinnerContext(DbContextOptions<DinnerContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //foreach (var type in modelBuilder.Model.GetEntityTypes(typeof(IRestaurant)))
        //    type.SetQueryFilter((IRestaurant e) => e.RestaurantId == AppScope.Restaurant.ResturantId);

        modelBuilder.Entity<Reservation>()
            .HasOne(x => x.Restaurant)
            .WithMany(x => x.Reservations)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Product>()
            .HasQueryFilter(x => x.RestaurantId == AppScope.Restaurant.ResturantId);

        base.OnModelCreating(modelBuilder);
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
            property.Entity.RestaurantId = AppScope.Restaurant.ResturantId;
            cancellationToken.ThrowIfCancellationRequested();
        }

        //foreach (var property in ChangeTracker.Entries<Entity>()
        //             .Where(w => w.State == EntityState.Added || w.State == EntityState.Modified))
        //{
        //    property.Entity. = PropertyId != 0 ? PropertyId : x.Entity.IdProperty;
        //    cancellationToken.ThrowIfCancellationRequested();
        //}
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetDefaults(cancellationToken);

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}