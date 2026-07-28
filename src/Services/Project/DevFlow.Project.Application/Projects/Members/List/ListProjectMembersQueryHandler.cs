using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.List;

internal sealed class ListProjectMembersQueryHandler
    : IRequestHandler<
        ListProjectMembersQuery,
        Result<IReadOnlyList<ListProjectMembersResponse>>>
{
    private readonly IProjectRepository _projectRepository;

    public ListProjectMembersQueryHandler(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<IReadOnlyList<ListProjectMembersResponse>>> Handle(
        ListProjectMembersQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<IReadOnlyList<ListProjectMembersResponse>>(
                ProjectErrors.NotFound);
        }

        var members = project.Members
            .Select(x => new ListProjectMembersResponse(
                x.UserId,
                x.Role.ToString(),
                x.JoinedOnUtc))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyList<ListProjectMembersResponse>>(
            members,
            "Project members retrieved successfully.");
    }
}
