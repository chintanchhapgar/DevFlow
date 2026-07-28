using DevFlow.Project.Domain.Projects.Enums;

namespace DevFlow.Project.Domain.Projects.Entities;

public sealed class ProjectInvitation
{
    private ProjectInvitation()
    {
    }

    private ProjectInvitation(
        string email,
        ProjectRole role,
        Guid invitedBy)
    {
        Id = Guid.NewGuid();
        Email = email.Trim().ToLowerInvariant();
        Role = role;
        InvitedBy = invitedBy;

        Status = InvitationStatus.Pending;

        Token = Guid.NewGuid();

        InvitedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddDays(7);
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public ProjectRole Role { get; private set; }

    public InvitationStatus Status { get; private set; }

    public Guid Token { get; private set; }

    public Guid InvitedBy { get; private set; }

    public DateTime InvitedOnUtc { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public DateTime? AcceptedOnUtc { get; private set; }

    public bool IsPending =>
        Status == InvitationStatus.Pending;

    public bool IsExpired =>
        DateTime.UtcNow > ExpiresOnUtc;

    public static ProjectInvitation Create(
        Guid projectId,
        string email,
        ProjectRole role,
        Guid invitedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return new ProjectInvitation(
            email,
            role,
            invitedBy);
    }

    public void Accept()
    {
        if (Status != InvitationStatus.Pending)
        {
            throw new InvalidOperationException(
                "Invitation has already been processed.");
        }

        if (ExpiresOnUtc <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Invitation has expired.");
        }

        Status = InvitationStatus.Accepted;
        AcceptedOnUtc = DateTime.UtcNow;
    }

    public void Revoke()
    {
        if (Status != InvitationStatus.Pending)
            throw new InvalidOperationException("Invitation cannot be revoked.");

        Status = InvitationStatus.Revoked;
    }

    public void Expire()
    {
        if (Status == InvitationStatus.Pending)
        {
            Status = InvitationStatus.Expired;
        }
    }

    public void Decline()
    {
        if (Status != InvitationStatus.Pending)
        {
            throw new InvalidOperationException(
                "Invitation has already been processed.");
        }

        if (ExpiresOnUtc <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Invitation has expired.");
        }

        Status = InvitationStatus.Declined;
    }
}
