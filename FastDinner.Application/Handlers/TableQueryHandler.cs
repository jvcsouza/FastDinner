using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Table;
using MediatR;

namespace FastDinner.Application.Handlers
{
    public class TableQueryHandler :
        IRequestHandler<TableQuery, IEnumerable<TableResponse>>,
        IRequestHandler<TableQueryById, TableResponse>
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

        public async Task<TableResponse> Handle(TableQueryById request, CancellationToken cancellationToken)
        {
            var table = await _tableRepository.GetByIdAsync(request.TableId);

            return new TableResponse(table.Id, table.Description, table.Seats);
        }
    }
}
