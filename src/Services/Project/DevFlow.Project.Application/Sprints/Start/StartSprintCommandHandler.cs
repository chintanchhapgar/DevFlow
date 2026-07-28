using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Sprints.Errors;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Start;

internal sealed class StartSprintCommandHandler
    : IRequestHandler<
        StartSprintCommand,
        Result<StartSprintResponse>>
{
    private readonly ISprintRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public StartSprintCommandHandler(
        ISprintRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<StartSprintResponse>> Handle(
        StartSprintCommand request,
        CancellationToken cancellationToken)
    {
        var sprint = await _repository.GetByIdAsync(
            new SprintId(request.SprintId),
            cancellationToken);

        if (sprint is null)
        {
            return Result.Failure<StartSprintResponse>(
                SprintErrors.NotFound);
        }

        try
        {
            sprint.Start();
        }
        catch (InvalidOperationException)
        {
            return Result.Failure<StartSprintResponse>(
                SprintErrors.InvalidState);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new StartSprintResponse(
                sprint.Id.Value,
                sprint.Status,
                sprint.StartedOnUtc!.Value),
            "Sprint started successfully.");
    }
}
