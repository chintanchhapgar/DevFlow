using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Sprints.Errors;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Complete;

internal sealed class CompleteSprintCommandHandler
    : IRequestHandler<
        CompleteSprintCommand,
        Result<CompleteSprintResponse>>
{
    private readonly ISprintRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteSprintCommandHandler(
        ISprintRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CompleteSprintResponse>> Handle(
        CompleteSprintCommand request,
        CancellationToken cancellationToken)
    {
        var sprint = await _repository.GetByIdAsync(
            new SprintId(request.SprintId),
            cancellationToken);

        if (sprint is null)
        {
            return Result.Failure<CompleteSprintResponse>(
                SprintErrors.NotFound);
        }

        try
        {
            sprint.Complete();
        }
        catch (InvalidOperationException)
        {
            return Result.Failure<CompleteSprintResponse>(
                SprintErrors.InvalidState);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CompleteSprintResponse(
                sprint.Id.Value,
                sprint.Status,
                sprint.CompletedOnUtc!.Value),
            "Sprint completed successfully.");
    }
}
