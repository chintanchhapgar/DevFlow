using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.GetAll;

internal sealed class GetProjectsQueryHandler
    : IRequestHandler<GetProjectsQuery, Result<GetProjectsResponse>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<GetProjectsResponse>> Handle(
        GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var (projects, totalCount) =
            await _projectRepository.GetPagedAsync(
                request.Page,
                request.PageSize,
                request.Search,
                cancellationToken);

        var items = projects
            .Select(project =>
                new ProjectListItemResponse(
                    project.Id.Value,
                    project.Key,
                    project.Name,
                    project.Status.ToString(),
                    project.Visibility.ToString(),
                    project.OwnerId,
                    project.Members.Count))
            .ToList();

        var response = new GetProjectsResponse(
            items,
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(
            response,
            "Projects retrieved successfully.");
    }
}
