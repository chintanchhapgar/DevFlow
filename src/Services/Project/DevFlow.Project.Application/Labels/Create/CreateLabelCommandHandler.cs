using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Labels.Entities;
using DevFlow.Project.Domain.Labels.Errors;
using DevFlow.Project.Domain.Labels.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.Create;

internal sealed class CreateLabelCommandHandler
    : IRequestHandler<
        CreateLabelCommand,
        Result<CreateLabelResponse>>
{
    private readonly ILabelRepository _labelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLabelCommandHandler(
        ILabelRepository labelRepository,
        IUnitOfWork unitOfWork)
    {
        _labelRepository = labelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateLabelResponse>> Handle(
        CreateLabelCommand request,
        CancellationToken cancellationToken)
    {
        var existing =
            await _labelRepository.GetByNameAsync(
                request.ProjectId,
                request.Name,
                cancellationToken);

        if (existing is not null)
        {
            return Result.Failure<CreateLabelResponse>(
                LabelErrors.DuplicateName);
        }

        var label = Label.Create(
            request.ProjectId,
            request.Name,
            request.Color);

        await _labelRepository.AddAsync(
            label,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateLabelResponse(
                label.Id.Value,
                label.ProjectId,
                label.Name,
                label.Color),
            "Label created successfully.");
    }
}
