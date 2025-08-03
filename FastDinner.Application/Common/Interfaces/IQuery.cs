using MediatR;

namespace FastDinner.Application.Common.Interfaces
{
    /// <summary>
    /// Marker interface for queries (read operations)
    /// </summary>
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
} 