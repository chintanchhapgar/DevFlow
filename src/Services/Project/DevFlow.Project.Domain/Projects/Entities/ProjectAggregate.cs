using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.Events;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Domain;
using DevFlow.SharedKernel.Exceptions;
using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Projects.Entities;

public sealed class ProjectAggregate
    : AggregateRoot<ProjectId>
{
    private readonly List<ProjectMember> _members = [];

    private ProjectAggregate(
        ProjectId id,
        string key,
        string name,
        string? description,
        Guid ownerId,
        ProjectVisibility visibility)
        : base(id)
    {
        Key = key;
        Name = name;
        Description = description;
        OwnerId = ownerId;
        Visibility = visibility;

        Status = ProjectStatus.Active;

        CreatedOnUtc = DateTime.UtcNow;

        _members.Add(
            ProjectMember.Create(
                ownerId,
                ProjectRole.Owner));

        RaiseDomainEvent(
    new ProjectCreatedDomainEvent(
        id,
        ownerId,
        name,
        description));
    }

    private ProjectAggregate()
        : base(ProjectId.Empty())
    {
    }

    public string Key { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Guid OwnerId { get; private set; }

    public ProjectVisibility Visibility { get; private set; }

    public ProjectStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public IReadOnlyCollection<ProjectMember> Members =>
        _members.AsReadOnly();

    private readonly List<ProjectInvitation> _invitations = [];

    public IReadOnlyCollection<ProjectInvitation> Invitations =>
    _invitations.AsReadOnly();
    public static ProjectAggregate Create(
        string key,
        string name,
        string? description,
        Guid ownerId,
        ProjectVisibility visibility)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        key = key.Trim().ToUpperInvariant();
        name = name.Trim();

        return new ProjectAggregate(
            ProjectId.New(),
            key,
            name,
            description?.Trim(),
            ownerId,
            visibility);
    }

    public void Update(
        string name,
        string? description,
        ProjectVisibility visibility)
    {
        if (Status == ProjectStatus.Archived)
            throw new InvalidOperationException(
                "Archived projects cannot be modified.");

        Name = name.Trim();

        Description = description?.Trim();

        Visibility = visibility;

        RaiseDomainEvent(
            new ProjectUpdatedDomainEvent(Id));
    }

    public void Archive()
    {
        if (Status == ProjectStatus.Archived)
            return;

        Status = ProjectStatus.Archived;

        RaiseDomainEvent(
            new ProjectArchivedDomainEvent(Id));
    }

    public void AddMember(
    Guid userId,
    ProjectRole role)
    {
        if (_members.Any(x => x.UserId == userId))
        {
            throw new InvalidOperationException(
                "User is already a member.");
        }

        _members.Add(
            ProjectMember.Create(
                userId,
                role));

        RaiseDomainEvent(
            new MemberAddedDomainEvent(
                Id,
                userId));
    }

    public Result RemoveMember(Guid userId)
    {
        var member = _members
            .FirstOrDefault(x => x.UserId == userId);

        if (member is null)
        {
            return Result.Failure(ProjectErrors.MemberNotFound);
        }

        if (member.UserId == OwnerId)
        {
            return Result.Failure(ProjectErrors.OwnerCannotBeRemoved);
        }

        _members.Remove(member);

        RaiseDomainEvent(
            new MemberRemovedDomainEvent(
                Id,
                userId));

        return Result.Success();
    }

    public Result ChangeMemberRole(
    Guid userId,
    ProjectRole role)
    {
        var member = _members
            .FirstOrDefault(x => x.UserId == userId);

        if (member is null)
        {
            return Result.Failure(ProjectErrors.MemberNotFound);
        }

        member.ChangeRole(role);

        RaiseDomainEvent(
            new MemberRoleChangedDomainEvent(
                Id,
                userId,
                role));

        return Result.Success();
    }

    public void Restore()
    {
        if (Status == ProjectStatus.Active)
            return;

        Status = ProjectStatus.Active;

        RaiseDomainEvent(
            new ProjectRestoredDomainEvent(Id));
    }

    public Result<ProjectInvitation> InviteMember(
    string email,
    ProjectRole role,
    Guid invitedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        email = email.Trim().ToLowerInvariant();

        if (_members.Any(x => x.UserId == invitedBy) == false)
        {
            return Result.Failure<ProjectInvitation>(
                ProjectErrors.Forbidden);
        }

        if (_invitations.Any(x =>
                x.Email == email &&
                x.Status == InvitationStatus.Pending))
        {
            return Result.Failure<ProjectInvitation>(
                ProjectErrors.InvitationAlreadyExists);
        }

        var invitation = ProjectInvitation.Create(
            Id.Value,
            email,
            role,
            invitedBy);

        _invitations.Add(invitation);

        RaiseDomainEvent(
            new ProjectInvitationCreatedDomainEvent(
                Id,
                invitation.Id));

        return Result.Success(invitation);
    }

    public Result AcceptInvitation(
    Guid token,
    Guid userId)
    {
        var invitation = _invitations
            .FirstOrDefault(x => x.Token == token);

        if (invitation is null)
        {
            return Result.Failure(ProjectErrors.InvitationNotFound);
        }

        if (invitation.Status != InvitationStatus.Pending)
        {
            return Result.Failure(ProjectErrors.InvitationAlreadyProcessed);
        }

        if (invitation.ExpiresOnUtc <= DateTime.UtcNow)
        {
            return Result.Failure(ProjectErrors.InvitationExpired);
        }

        if (_members.Any(x => x.UserId == userId))
        {
            return Result.Failure(ProjectErrors.MemberAlreadyExists);
        }

        _members.Add(
            ProjectMember.Create(
                userId,
                invitation.Role));

        invitation.Accept();

        RaiseDomainEvent(
            new ProjectInvitationAcceptedDomainEvent(
                Id,
                invitation.Id,
                userId));

        return Result.Success();
    }

    public Result RevokeInvitation(Guid invitationId)
    {
        var invitation = _invitations
            .FirstOrDefault(x => x.Id == invitationId);

        if (invitation is null)
        {
            return Result.Failure(
                ProjectErrors.InvitationNotFound);
        }

        invitation.Revoke();

        RaiseDomainEvent(
            new ProjectInvitationRevokedDomainEvent(
                Id,
                invitation.Id));

        return Result.Success();
    }


    public Result DeclineInvitation(Guid token)
    {
        var invitation = _invitations
            .FirstOrDefault(x => x.Token == token);

        if (invitation is null)
        {
            return Result.Failure(
                ProjectErrors.InvitationNotFound);
        }

        if (invitation.Status != InvitationStatus.Pending)
        {
            return Result.Failure(
                ProjectErrors.InvitationAlreadyProcessed);
        }

        invitation.Decline();

        RaiseDomainEvent(
            new ProjectInvitationDeclinedDomainEvent(
                Id,
                invitation.Id));

        return Result.Success();
    }

}
