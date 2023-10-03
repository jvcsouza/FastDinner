using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public class RestaurantRepository : BaseRepository<Restaurant>, IRestaurantRepository
{
    public RestaurantRepository(DinnerContext context) : base(context)
    {

    }
}