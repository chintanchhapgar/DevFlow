using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Labels.Errors;
using DevFlow.Project.Domain.Labels.Repositories;
using DevFlow.Project.Domain.Labels.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.Delete;

internal sealed class DeleteLabelCommandHandler
    : IRequestHandler<
        DeleteLabelCommand,
        Result<DeleteLabelResponse>>
{
    private readonly ILabelRepository _labelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLabelCommandHandler(
        ILabelRepository labelRepository,
        IUnitOfWork unitOfWork)
    {
        _labelRepository = labelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeleteLabelResponse>> Handle(
        DeleteLabelCommand request,
        CancellationToken cancellationToken)
    {
        var label =
            await _labelRepository.GetByIdAsync(
                new LabelId(request.LabelId),
                cancellationToken);

        if (label is null)
        {
            return Result.Failure<DeleteLabelResponse>(
                LabelErrors.NotFound);
        }

        if (label.IsDeleted)
        {
            return Result.Failure<DeleteLabelResponse>(
                LabelErrors.AlreadyDeleted);
        }

        label.Delete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new DeleteLabelResponse(
                label.Id.Value,
                label.ProjectId,
                label.Name,
                DateTime.UtcNow),
            "Label deleted successfully.");
    }
}
