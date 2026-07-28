namespace DevFlow.Project.Application.Projects.Members.Remove;

public sealed record RemoveProjectMemberResponse(
    Guid ProjectId,
    Guid UserId);
