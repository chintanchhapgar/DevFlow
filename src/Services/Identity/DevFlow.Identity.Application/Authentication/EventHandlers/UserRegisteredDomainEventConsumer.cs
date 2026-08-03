using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using DevFlow.Identity.Contracts.IntegrationEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Application.Authentication.DomainEvents;

public sealed class UserRegisteredDomainEventConsumer
    : IDomainEventConsumer<UserRegisteredDomainEvent>
{
    private readonly IIntegrationEventPublisher _publisher;

    public UserRegisteredDomainEventConsumer(
        IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task ConsumeAsync(
        UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var integrationEvent = new UserRegisteredIntegrationEvent(
            domainEvent.UserId.Value,
            domainEvent.Email,
            domainEvent.FirstName,
            domainEvent.LastName);

        await _publisher.PublishAsync(
            integrationEvent,
            cancellationToken);
    }
}
