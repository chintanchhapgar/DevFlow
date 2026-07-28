using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Sprints.Errors;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Delete;

internal sealed class DeleteSprintCommandHandler
    : IRequestHandler<
        DeleteSprintCommand,
        Result<DeleteSprintResponse>>
{
    private readonly ISprintRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSprintCommandHandler(
        ISprintRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeleteSprintResponse>> Handle(
        DeleteSprintCommand request,
        CancellationToken cancellationToken)
    {
        var sprint = await _repository.GetByIdAsync(
            new SprintId(request.SprintId),
            cancellationToken);

        if (sprint is null)
        {
            return Result.Failure<DeleteSprintResponse>(
                SprintErrors.NotFound);
        }

        sprint.Delete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new DeleteSprintResponse(
                sprint.Id.Value,
                "Deleted"),
            "Sprint deleted successfully.");
    }
}
