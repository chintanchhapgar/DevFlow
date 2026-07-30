using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Epics.Errors;
using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.Project.Domain.Epics.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.Delete;

internal sealed class DeleteEpicCommandHandler
    : IRequestHandler<
        DeleteEpicCommand,
        Result<DeleteEpicResponse>>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEpicCommandHandler(
        IEpicRepository epicRepository,
        IUnitOfWork unitOfWork)
    {
        _epicRepository = epicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeleteEpicResponse>> Handle(
        DeleteEpicCommand request,
        CancellationToken cancellationToken)
    {
        var epic =
            await _epicRepository.GetByIdAsync(
                new EpicId(request.EpicId),
                cancellationToken);

        if (epic is null)
        {
            return Result.Failure<DeleteEpicResponse>(
                EpicErrors.NotFound);
        }

        if (epic.IsDeleted)
        {
            return Result.Failure<DeleteEpicResponse>(
                EpicErrors.AlreadyDeleted);
        }

        epic.Delete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new DeleteEpicResponse(
                epic.Id.Value,
                epic.ProjectId,
                epic.Name,
                DateTime.UtcNow),
            "Epic deleted successfully.");
    }
}
