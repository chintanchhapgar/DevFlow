using MassTransit;
using System.Text.RegularExpressions;

namespace DevFlow.BuildingBlocks.Messaging.Configuration;

/// <summary>
/// Formats MassTransit entity names (exchanges/queues) in kebab-case.
/// Example: UserRegisteredIntegrationEvent -> user-registered-integration-event
/// </summary>
public sealed partial class KebabCaseEntityNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return ToKebabCase(typeof(T).Name);
    }

    private static string ToKebabCase(string name)
    {
        return KebabCaseRegex()
            .Replace(name, "-$1")
            .ToLowerInvariant()
            .TrimStart('-');
    }

    [GeneratedRegex("([A-Z])")]
    private static partial Regex KebabCaseRegex();
}
