using FastDinner.Application.Common;
using FastDinner.Domain.Model;
using FastDinner.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

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

        modelBuilder.Entity<Product>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Employee>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Menu>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Order>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);
        
        modelBuilder.Entity<Reservation>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);

        modelBuilder.Entity<Table>()
            .HasQueryFilter(x => x.RestaurantId == _appScope.RestaurantId);
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