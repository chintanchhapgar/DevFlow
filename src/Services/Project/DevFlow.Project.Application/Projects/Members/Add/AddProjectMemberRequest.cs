using DevFlow.Project.Domain.Projects.Enums;

namespace DevFlow.Project.Application.Projects.Members.Add;

public sealed record AddProjectMemberRequest(
    Guid UserId,
    ProjectRole Role);
