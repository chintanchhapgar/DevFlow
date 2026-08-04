using DevFlow.BuildingBlocks.Messaging.Outbox;
using DevFlow.BuildingBlocks.Messaging.Serialization;
using DevFlow.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevFlow.Identity.Infrastructure.Outbox;

internal sealed class IdentityOutboxProcessor
    : OutboxProcessor<IdentityDbContext>
{
    public IdentityOutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<IdentityOutboxProcessor> logger,
        IMessageSerializer serializer,
        IIntegrationEventTypeResolver eventTypeResolver)
        : base(
            scopeFactory,
            logger,
            serializer,
            eventTypeResolver)
    {
    }

    protected override async Task SaveChangesAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var context = serviceProvider.GetRequiredService<IdentityDbContext>();

        await context.SaveChangesAsync(cancellationToken);
    }
}
