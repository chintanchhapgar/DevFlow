namespace DevFlow.Project.Application.Projects.GetAll;

public sealed record GetProjectsResponse(
    IReadOnlyCollection<ProjectListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount)
{
    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);
}
