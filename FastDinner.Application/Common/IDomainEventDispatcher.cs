using FastDinner.Domain.Events;

namespace FastDinner.Application.Common;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}
