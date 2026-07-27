
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.GetById;

public sealed record GetProjectByIdQuery(
    Guid ProjectId)
    : IRequest<Result<GetProjectResponse>>;
