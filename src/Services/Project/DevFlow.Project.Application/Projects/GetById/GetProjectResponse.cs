namespace DevFlow.Project.Application.Projects.GetById;

public sealed record GetProjectResponse(
    Guid ProjectId,
    string Key,
    string Name,
    string? Description,
    string Status,
    string Visibility,
    Guid OwnerId,
    string OwnerName,
    IReadOnlyCollection<ProjectMemberResponse> Members);

public sealed record ProjectMemberResponse(
    Guid UserId,
    string Role,
    string MemberName,
    DateTime JoinedOnUtc);
