using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Labels.Errors;
using DevFlow.Project.Domain.Labels.Repositories;
using DevFlow.Project.Domain.Labels.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.Update;

internal sealed class UpdateLabelCommandHandler
    : IRequestHandler<
        UpdateLabelCommand,
        Result<UpdateLabelResponse>>
{
    private readonly ILabelRepository _labelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLabelCommandHandler(
        ILabelRepository labelRepository,
        IUnitOfWork unitOfWork)
    {
        _labelRepository = labelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateLabelResponse>> Handle(
        UpdateLabelCommand request,
        CancellationToken cancellationToken)
    {
        var label = await _labelRepository.GetByIdAsync(
            new LabelId(request.LabelId),
            cancellationToken);

        if (label is null)
        {
            return Result.Failure<UpdateLabelResponse>(
                LabelErrors.NotFound);
        }

        var duplicate =
            await _labelRepository.GetByNameAsync(
                label.ProjectId,
                request.Name,
                cancellationToken);

        if (duplicate is not null &&
            duplicate.Id != label.Id)
        {
            return Result.Failure<UpdateLabelResponse>(
                LabelErrors.DuplicateName);
        }

        label.Rename(request.Name);
        label.ChangeColor(request.Color);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UpdateLabelResponse(
                label.Id.Value,
                label.ProjectId,
                label.Name,
                label.Color),
            "Label updated successfully.");
    }
}
