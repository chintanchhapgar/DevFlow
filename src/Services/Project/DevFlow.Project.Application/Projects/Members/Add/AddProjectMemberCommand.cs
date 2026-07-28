using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.Add;

public sealed record AddProjectMemberCommand(
    Guid ProjectId,
    Guid UserId,
    ProjectRole Role)
    : IRequest<Result<AddProjectMemberResponse>>;
