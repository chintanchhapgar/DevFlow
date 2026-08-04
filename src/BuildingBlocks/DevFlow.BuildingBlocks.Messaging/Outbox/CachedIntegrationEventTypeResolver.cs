using System.Reflection;

namespace DevFlow.BuildingBlocks.Messaging.Outbox;

public sealed class CachedIntegrationEventTypeResolver
    : IIntegrationEventTypeResolver
{
    public Type Resolve(string eventTypeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventTypeName);

        // Try using the assembly-qualified name
        var type = Type.GetType(eventTypeName, throwOnError: false);

        if (type is not null)
        {
            return type;
        }

        // Fallback: search loaded assemblies
        var fullName = eventTypeName.Split(',')[0];

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(fullName, false);

            if (type is not null)
            {
                return type;
            }
        }

        throw new InvalidOperationException(
            $"Unable to resolve integration event type '{eventTypeName}'.");
    }
}
