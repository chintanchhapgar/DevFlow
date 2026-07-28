using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.GetAll;

public sealed record GetProjectInvitationsQuery(
    Guid ProjectId)
    : IRequest<Result<IReadOnlyList<GetProjectInvitationsResponse>>>;
