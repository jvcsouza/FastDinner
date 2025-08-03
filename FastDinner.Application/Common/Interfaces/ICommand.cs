using MediatR;

namespace FastDinner.Application.Common.Interfaces
{
    /// <summary>
    /// Marker interface for commands (write operations)
    /// </summary>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
} 