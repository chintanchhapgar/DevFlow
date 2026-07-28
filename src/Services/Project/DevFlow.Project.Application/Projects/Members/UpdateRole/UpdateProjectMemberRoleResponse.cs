namespace DevFlow.Project.Application.Projects.Members.UpdateRole;

public sealed record UpdateProjectMemberRoleResponse(
    Guid ProjectId,
    Guid UserId,
    string Role);
