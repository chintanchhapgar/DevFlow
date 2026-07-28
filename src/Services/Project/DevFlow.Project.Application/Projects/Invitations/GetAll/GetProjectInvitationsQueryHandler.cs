using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.GetAll;

internal sealed class GetProjectInvitationsQueryHandler
    : IRequestHandler<
        GetProjectInvitationsQuery,
        Result<IReadOnlyList<GetProjectInvitationsResponse>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUser _currentUser;

    public GetProjectInvitationsQueryHandler(
        IProjectRepository projectRepository,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyList<GetProjectInvitationsResponse>>> Handle(
        GetProjectInvitationsQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<IReadOnlyList<GetProjectInvitationsResponse>>(
                ProjectErrors.NotFound);
        }

        // Only project owner can view invitations
        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<IReadOnlyList<GetProjectInvitationsResponse>>(
                ProjectErrors.Forbidden);
        }

        var invitations = project.Invitations
            .OrderByDescending(x => x.InvitedOnUtc)
            .Select(x => new GetProjectInvitationsResponse(
                x.Id,
                x.Email,
                x.Role.ToString(),
                x.Status.ToString(),
                x.Token,
                x.InvitedBy,
                x.InvitedOnUtc,
                x.ExpiresOnUtc,
                x.AcceptedOnUtc))
            .ToList();

        return Result.Success<IReadOnlyList<GetProjectInvitationsResponse>>(
            invitations,
            "Project invitations retrieved successfully.");
    }
}
