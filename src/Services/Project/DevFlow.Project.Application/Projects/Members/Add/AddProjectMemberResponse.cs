namespace DevFlow.Project.Application.Projects.Members.Add;

public sealed record AddProjectMemberResponse(
    Guid ProjectId,
    Guid UserId,
    string Role);
