using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.Create;

public sealed record CreateLabelCommand(
    Guid ProjectId,
    string Name,
    string Color)
    : IRequest<Result<CreateLabelResponse>>;
