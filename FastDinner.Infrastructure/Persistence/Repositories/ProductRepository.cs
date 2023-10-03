using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(DinnerContext context) : base(context)
    {

    }
}