using FastDinner.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence;

public class DinnerContext : DbContext
{
    public DinnerContext(DbContextOptions<DinnerContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}