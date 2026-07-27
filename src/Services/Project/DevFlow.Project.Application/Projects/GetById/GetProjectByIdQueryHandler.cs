using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.GetById;

internal sealed class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, Result<GetProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdQueryHandler(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<GetProjectResponse>> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<GetProjectResponse>(
                ProjectErrors.NotFound);
        }

        var response = new GetProjectResponse(
            project.Id.Value,
            project.Key,
            project.Name,
            project.Description,
            project.Status.ToString(),
            project.Visibility.ToString(),
            project.OwnerId,
            project.Members
                .Select(member =>
                    new ProjectMemberResponse(
                        member.UserId,
                        member.Role.ToString(),
                        member.JoinedOnUtc))
                .ToList());

        return Result.Success(
            response,
            "Project retrieved successfully.");
    }
}
