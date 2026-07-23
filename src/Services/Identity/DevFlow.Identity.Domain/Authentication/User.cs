using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication;

/// <summary>
/// Represents a system user.
/// </summary>
public sealed class User : AggregateRoot<UserId>
{
    private User(
        UserId id,
        string email,
        string passwordHash,
        string firstName,
        string lastName)
        : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;

        Role = UserRole.Member;
        Status = UserStatus.Active;
        EmailConfirmed = true;
        CreatedOnUtc = DateTime.UtcNow;
    }

    // Required by EF Core
    private User()
    {
    }

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public UserStatus Status { get; private set; } = UserStatus.Active;

    public bool EmailConfirmed { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public bool IsActive => Status == UserStatus.Active;
    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        var user = new User(
            UserId.New(),
            email.Trim().ToLowerInvariant(),
            passwordHash,
            firstName.Trim(),
            lastName.Trim());

        user.RaiseDomainEvent(
            new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        Status = UserStatus.Active;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ChangePassword(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        PasswordHash = passwordHash;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateProfile(
        string firstName,
        string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Disabled;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}
