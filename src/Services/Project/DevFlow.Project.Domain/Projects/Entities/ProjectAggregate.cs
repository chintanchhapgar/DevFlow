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
            new ProjectCreatedDomainEvent(id));
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


}
