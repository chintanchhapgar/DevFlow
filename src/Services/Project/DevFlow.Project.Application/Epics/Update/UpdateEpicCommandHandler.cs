using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Epics.Errors;
using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.Project.Domain.Epics.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.Update;

internal sealed class UpdateEpicCommandHandler
    : IRequestHandler<
        UpdateEpicCommand,
        Result<UpdateEpicResponse>>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEpicCommandHandler(
        IEpicRepository epicRepository,
        IUnitOfWork unitOfWork)
    {
        _epicRepository = epicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateEpicResponse>> Handle(
        UpdateEpicCommand request,
        CancellationToken cancellationToken)
    {
        var epic =
            await _epicRepository.GetByIdAsync(
                new EpicId(request.EpicId),
                cancellationToken);

        if (epic is null)
        {
            return Result.Failure<UpdateEpicResponse>(
                EpicErrors.NotFound);
        }

        var duplicate =
            await _epicRepository.GetByNameAsync(
                epic.ProjectId,
                request.Name,
                cancellationToken);

        if (duplicate is not null &&
            duplicate.Id != epic.Id)
        {
            return Result.Failure<UpdateEpicResponse>(
                EpicErrors.DuplicateName);
        }

        epic.Update(
            request.Name,
            request.Description,
            request.Color,
            request.StartDate,
            request.DueDate);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UpdateEpicResponse(
                epic.Id.Value,
                epic.ProjectId,
                epic.Name,
                epic.Description,
                epic.Color,
                epic.StartDate,
                epic.DueDate),
            "Epic updated successfully.");
    }
}
