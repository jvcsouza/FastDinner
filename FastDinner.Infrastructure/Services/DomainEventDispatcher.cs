using FastDinner.Application.Common;
using FastDinner.Domain.Events;
using MediatR;

namespace FastDinner.Infrastructure.Services;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in events)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
