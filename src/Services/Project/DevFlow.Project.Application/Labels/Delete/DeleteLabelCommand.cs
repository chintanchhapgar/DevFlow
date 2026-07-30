using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.Delete;

public sealed record DeleteLabelCommand(
    Guid LabelId)
    : IRequest<Result<DeleteLabelResponse>>;
