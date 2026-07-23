namespace DevFlow.Identity.Application.Common.Abstractions.Messaging;

/// <summary>
/// Publishes integration events.
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken = default)
        where T : class;
}
