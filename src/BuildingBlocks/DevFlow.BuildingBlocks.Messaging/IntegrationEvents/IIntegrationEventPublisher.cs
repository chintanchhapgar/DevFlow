using System.Threading;
using System.Threading.Tasks;

namespace DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

/// <summary>
/// Publishes integration events by storing them
/// in the transactional outbox.
/// </summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default);
}
