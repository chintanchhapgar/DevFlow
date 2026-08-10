using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Users;
using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Application.Authentication.ResendVerification;

public sealed class EmailVerificationResentDomainEventConsumer
    : IDomainEventConsumer<EmailVerificationResentDomainEvent>
{
    private readonly IIntegrationEventPublisher _publisher;

    public EmailVerificationResentDomainEventConsumer(
        IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task ConsumeAsync(
        EmailVerificationResentDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await _publisher.PublishAsync(
            new UserEmailVerificationResentIntegrationEvent(
                domainEvent.UserId.Value,
                domainEvent.Email,
                domainEvent.FirstName,
                domainEvent.VerificationToken),
            cancellationToken);
    }
}
