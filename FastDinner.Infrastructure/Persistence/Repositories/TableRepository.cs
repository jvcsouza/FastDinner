using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;

namespace FastDinner.Infrastructure.Persistence.Repositories
{
    public class TableRepository : BaseRepository<Table>, ITableRepository
    {
        public TableRepository(DinnerContext context) : base(context)
        {
        }
    }
}
