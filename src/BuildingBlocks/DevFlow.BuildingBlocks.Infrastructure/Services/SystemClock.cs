using DevFlow.SharedKernel.Common;

namespace DevFlow.BuildingBlocks.Infrastructure.Services;

/// <summary>
/// Production implementation of IClock using system time.
/// </summary>
public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
}
