using DevFlow.Project.Domain.Projects.Enums;

namespace DevFlow.Project.Domain.Projects.Entities;

public sealed class ProjectMember
{
    private ProjectMember() { }

    private ProjectMember(Guid userId, ProjectRole role)
    {
        Id = Guid.NewGuid(); // ✅ Surrogate key
        UserId = userId;
        Role = role;
        JoinedOnUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; } // ✅ Add this

    public Guid UserId { get; private set; }

    public ProjectRole Role { get; private set; }

    public DateTime JoinedOnUtc { get; private set; }

    public static ProjectMember Create(Guid userId, ProjectRole role)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));

        return new ProjectMember(userId, role);
    }

    public void ChangeRole(ProjectRole role) => Role = role;
}
