using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Pagination;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.GetAll;

internal sealed class GetProjectsQueryHandler
    : IRequestHandler<
        GetProjectsQuery,
        Result<PagedList<ProjectListItemResponse>>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<PagedList<ProjectListItemResponse>>> Handle(
        GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var pagedProjects =
            await _projectRepository.GetPagedAsync(
                request.Pagination,
                request.Search,
                cancellationToken);

        var response =
            pagedProjects.Map(project =>
                new ProjectListItemResponse(
                    project.Id.Value,
                    project.Key,
                    project.Name,
                    project.Status.ToString(),
                    project.Visibility.ToString(),
                    project.OwnerId,
                    project.Members.Count));

        return Result.Success(
            response,
            "Projects retrieved successfully.");
    }
}
