using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.Remove;

public sealed record RemoveProjectMemberCommand(
    Guid ProjectId,
    Guid UserId)
    : IRequest<Result<RemoveProjectMemberResponse>>;
