namespace DevFlow.Project.Application.Projects.Members.List;

public sealed record ListProjectMembersResponse(
    Guid UserId,
    string Role,
    DateTime JoinedOnUtc);
