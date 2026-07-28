using DevFlow.Project.Domain.Sprints.Errors;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.GetById;

internal sealed class GetSprintByIdQueryHandler
    : IRequestHandler<
        GetSprintByIdQuery,
        Result<GetSprintByIdResponse>>
{
    private readonly ISprintRepository _repository;

    public GetSprintByIdQueryHandler(
        ISprintRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetSprintByIdResponse>> Handle(
        GetSprintByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sprint = await _repository.GetByIdAsync(
            new SprintId(request.SprintId),
            cancellationToken);

        if (sprint is null)
        {
            return Result.Failure<GetSprintByIdResponse>(
                SprintErrors.NotFound);
        }

        return Result.Success(
            new GetSprintByIdResponse(
                sprint.Id.Value,
                sprint.ProjectId,
                sprint.Name,
                sprint.Goal,
                sprint.Status,
                sprint.StartDate,
                sprint.EndDate,
                sprint.StartedOnUtc,
                sprint.CompletedOnUtc,
                sprint.CreatedOnUtc));
    }
}
