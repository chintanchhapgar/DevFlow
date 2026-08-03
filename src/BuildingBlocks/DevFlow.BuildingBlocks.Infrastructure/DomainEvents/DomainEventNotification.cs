using DevFlow.SharedKernel.Domain.DomainEvents;
using MediatR;

namespace DevFlow.BuildingBlocks.Infrastructure.DomainEvents;

/// <summary>
/// Wraps a domain event so it can be published through MediatR.
/// </summary>
public sealed record DomainEventNotification(
    IDomainEvent DomainEvent)
    : INotification;
