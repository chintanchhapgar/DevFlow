using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Update;

public sealed record UpdateProjectCommand(
    Guid ProjectId,
    string Name,
    string? Description,
    ProjectVisibility Visibility)
    : IRequest<Result<UpdateProjectResponse>>;
