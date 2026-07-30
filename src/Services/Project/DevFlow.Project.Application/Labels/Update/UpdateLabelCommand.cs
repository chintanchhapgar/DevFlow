using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.Update;

public sealed record UpdateLabelCommand(
    Guid LabelId,
    string Name,
    string Color)
    : IRequest<Result<UpdateLabelResponse>>;
