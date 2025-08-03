using FastDinner.Application.Common.Interfaces;
using FastDinner.Application.Common.Interfaces.Repositories;
using MediatR;

namespace FastDinner.Application.Common
{
    internal class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Only apply transactions for Commands (write operations)
            // Queries (read operations) don't need transactions
            if (request is ICommand<TResponse>)
            {
                return await _unitOfWork.ExecuteTransactionAsync(async () => await next());
            }

            // For queries, just execute without transaction
            return await next();
        }
    }
}
