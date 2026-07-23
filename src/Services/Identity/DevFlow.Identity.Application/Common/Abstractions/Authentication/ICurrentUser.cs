using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Represents the currently authenticated user.
/// </summary>
public interface ICurrentUser
{
    UserId UserId { get; }

    string Email { get; }

    string Name { get; }

    string Role { get; }

    bool IsAuthenticated { get; }
}
