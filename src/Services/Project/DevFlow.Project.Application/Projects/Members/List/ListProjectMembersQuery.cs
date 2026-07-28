using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.List;

public sealed record ListProjectMembersQuery(
    Guid ProjectId)
    : IRequest<Result<IReadOnlyList<ListProjectMembersResponse>>>;
