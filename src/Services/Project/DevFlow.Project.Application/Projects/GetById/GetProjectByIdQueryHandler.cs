using DevFlow.Project.Application.Common.Abstractions.Identity;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Results;
using DevFlow.SharedKernel.Common;
using MediatR;

namespace DevFlow.Project.Application.Projects.GetById;

internal sealed class GetProjectByIdQueryHandler
    : IRequestHandler<
        GetProjectByIdQuery,
        Result<GetProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserLookupService _userLookupService;
    private readonly ICurrentUser _currentUser;

    public GetProjectByIdQueryHandler(
        IProjectRepository projectRepository,
        IUserLookupService userLookupService,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _userLookupService = userLookupService;
        _currentUser = currentUser;
    }

    public async Task<Result<GetProjectResponse>> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var project =
            await _projectRepository.GetByIdAsync(
                new ProjectId(request.ProjectId),
                cancellationToken);

        if (project is null)
        {
            return Result.Failure<GetProjectResponse>(
                ProjectErrors.NotFound);
        }

        if (_currentUser.Role == UserRole.Member.ToString() &&
            project.Members.All(member => member.UserId != _currentUser.UserId))
        {
            return Result.Failure<GetProjectResponse>(ProjectErrors.Forbidden);
        }

        // Collect owner + member IDs and remove duplicates.
        var userIds = project.Members
            .Select(member => member.UserId)
            .Append(project.OwnerId)
            .Distinct()
            .ToArray();

        // Resolve all names with a single Identity API call.
        var userNames =
            await _userLookupService.GetNamesAsync(
                userIds,
                cancellationToken);

        // Resolve owner name.
        var ownerName =
            userNames.TryGetValue(
                project.OwnerId,
                out var resolvedOwnerName)
                ? resolvedOwnerName
                : "Unknown User";

        // Resolve member names.
        var members =
            project.Members
                .Select(member =>
                {
                    var memberName =
                        userNames.TryGetValue(
                            member.UserId,
                            out var resolvedMemberName)
                            ? resolvedMemberName
                            : "Unknown User";

                    return new ProjectMemberResponse(
                        member.UserId,
                        member.Role.ToString(),
                        memberName,
                        member.JoinedOnUtc);
                })
                .ToList();

        var response = new GetProjectResponse(
            project.Id.Value,
            project.Key,
            project.Name,
            project.Description,
            project.Status.ToString(),
            project.Visibility.ToString(),
            project.OwnerId,
            ownerName,
            members);

        return Result.Success(
            response,
            "Project retrieved successfully.");
    }
}
