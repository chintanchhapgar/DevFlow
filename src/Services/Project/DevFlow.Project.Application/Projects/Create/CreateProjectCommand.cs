using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Create;

public sealed record CreateProjectCommand(
    string Key,
    string Name,
    string? Description,
    ProjectVisibility Visibility)
    : IRequest<Result<CreateProjectResponse>>;
