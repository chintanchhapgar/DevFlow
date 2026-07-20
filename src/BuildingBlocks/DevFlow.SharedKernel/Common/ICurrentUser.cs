namespace DevFlow.SharedKernel.Common;

/// <summary>
/// Provides access to the currently authenticated user's context.
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// The unique identifier of the current user.
    /// Returns null for unauthenticated requests.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// The email address of the current user.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Whether the current request is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }
}
