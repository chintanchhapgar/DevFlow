using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Sprints.Errors;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Update;

internal sealed class UpdateSprintCommandHandler
    : IRequestHandler<
        UpdateSprintCommand,
        Result<UpdateSprintResponse>>
{
    private readonly ISprintRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSprintCommandHandler(
        ISprintRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateSprintResponse>> Handle(
        UpdateSprintCommand request,
        CancellationToken cancellationToken)
    {
        var sprint = await _repository.GetByIdAsync(
            new SprintId(request.SprintId),
            cancellationToken);

        if (sprint is null)
        {
            return Result.Failure<UpdateSprintResponse>(
                SprintErrors.NotFound);
        }

        sprint.Update(
            request.Name,
            request.Goal,
            request.StartDate,
            request.EndDate);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UpdateSprintResponse(
                sprint.Id.Value,
                sprint.Name),
            "Sprint updated successfully.");
    }
}
