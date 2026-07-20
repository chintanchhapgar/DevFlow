namespace DevFlow.Identity.Domain.Roles;

/// <summary>
/// System-level roles for authorization.
/// Kept as enum for simplicity — extend to entity if roles become dynamic.
/// </summary>
public enum Role
{
    User = 1,
    Admin = 2
}
