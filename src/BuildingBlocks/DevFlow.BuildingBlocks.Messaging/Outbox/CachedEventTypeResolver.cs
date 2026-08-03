using System.Collections.Concurrent;
using System.Reflection;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.BuildingBlocks.Messaging.Outbox;

/// <summary>
/// Resolves domain event CLR types using a cached lookup.
/// </summary>
public sealed class CachedEventTypeResolver : IEventTypeResolver
{
    private readonly Dictionary<string, Type> _types;

    public CachedEventTypeResolver()
    {
        _types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Where(t =>
                !t.IsAbstract &&
                typeof(IDomainEvent).IsAssignableFrom(t))
            .ToDictionary(
                t => t.AssemblyQualifiedName!,
                t => t);
    }

    public Type Resolve(string eventTypeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventTypeName);

        if (_types.TryGetValue(eventTypeName, out var type))
        {
            return type;
        }

        throw new InvalidOperationException(
            $"Unable to resolve domain event type '{eventTypeName}'.");
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t is not null)!;
        }
    }
}
