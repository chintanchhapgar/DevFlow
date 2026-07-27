using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.Project.Domain.Projects.Events;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Domain;
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

    public Result Archive()
    {
        if (Status == ProjectStatus.Archived)
            return Result.Success();

        Status = ProjectStatus.Archived;
        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new ProjectArchivedDomainEvent(Id));

        return Result.Success();
    }

    public Result AddMember(
        Guid userId,
        ProjectRole role)
    {
        if (Status == ProjectStatus.Archived)
            return Result.Failure(ProjectErrors.Archived);

        if (_members.Any(x => x.UserId == userId))
            return Result.Failure(ProjectErrors.MemberAlreadyExists);

        _members.Add(
            ProjectMember.Create(
                userId,
                role));

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new MemberAddedDomainEvent(
                Id,
                userId));

        return Result.Success();
    }

    public Result RemoveMember(Guid userId)
    {
        if (userId == OwnerId)
            return Result.Failure(ProjectErrors.CannotRemoveOwner);

        var member = _members.FirstOrDefault(
            x => x.UserId == userId);

        if (member is null)
            return Result.Failure(ProjectErrors.MemberNotFound);

        _members.Remove(member);

        UpdatedOnUtc = DateTime.UtcNow;

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
        var member = _members.FirstOrDefault(
            x => x.UserId == userId);

        if (member is null)
            return Result.Failure(ProjectErrors.MemberNotFound);

        member.ChangeRole(role);

        UpdatedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }
}
