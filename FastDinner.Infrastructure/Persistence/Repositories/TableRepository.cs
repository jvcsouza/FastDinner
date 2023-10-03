using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDinner.Infrastructure.Persistence.Repositories
{
    public class TableRepository : BaseRepository<Table>, ITableRepository
    {
        public TableRepository(DinnerContext context) : base(context)
        {
        }
    }
}
