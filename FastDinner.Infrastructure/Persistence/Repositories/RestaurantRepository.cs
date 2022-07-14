using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly DinnerContext _context;

    public RestaurantRepository(DinnerContext context)
    {
        _context = context;
    }

    public async Task<Restaurant> Create(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant);
        return restaurant;
    }

    public async Task<IEnumerable<Restaurant>> GetAll()
    {
        return await _context.Restaurants.ToListAsync();
    }

    public async Task<Restaurant> GetById(Guid id)
    {
        return await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Restaurant> Update(Restaurant entity)
    {
        throw new NotImplementedException();
    }
}