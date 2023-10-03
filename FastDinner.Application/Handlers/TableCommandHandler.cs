using FastDinner.Application.Commands.Table;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Contracts.Table;
using FastDinner.Domain.Model;
using MediatR;

namespace FastDinner.Application.Handlers
{
    public class TableCommandHandler :
         IRequestHandler<CreateTableCommand, TableResponse>
    {
        private readonly ITableRepository _tableRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TableCommandHandler(ITableRepository tableRepository, IUnitOfWork unitOfWork)
        {
            _tableRepository = tableRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TableResponse> Handle(CreateTableCommand request, CancellationToken cancellationToken)
        {
            var table = await _tableRepository.CreateAsync(new Table(
                request.Description,
                request.Seats
            ));

            await _unitOfWork.CommitAsync();

            return new TableResponse(table.Id, table.Description, table.Seats);
        }
    }
}
