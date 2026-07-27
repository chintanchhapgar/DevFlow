namespace DevFlow.Project.Application.Projects.GetAll;

public sealed record ProjectListItemResponse(
    Guid ProjectId,
    string Key,
    string Name,
    string Status,
    string Visibility,
    Guid OwnerId,
    int MemberCount);
