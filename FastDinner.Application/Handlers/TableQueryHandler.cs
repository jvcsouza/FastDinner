using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Table;
using MediatR;

namespace FastDinner.Application.Handlers
{
    public class TableQueryHandler :
        IRequestHandler<TableQuery, IEnumerable<TableResponse>>
    {
        private readonly ITableRepository _tableRepository;

        public TableQueryHandler(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public async Task<IEnumerable<TableResponse>> Handle(TableQuery request, CancellationToken cancellationToken)
        {
            var tables = await _tableRepository.GetAllAsync();

            return tables.Select(x => new TableResponse(x.Id, x.Description, x.Seats));
        }
    }
}
