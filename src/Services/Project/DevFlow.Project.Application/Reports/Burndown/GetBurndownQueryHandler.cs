using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Reports.Burndown;
using DevFlow.Project.Domain.Sprints.Errors;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Burndown;

internal sealed class GetBurndownQueryHandler
    : IRequestHandler<
        GetBurndownQuery,
        Result<GetBurndownResponse>>
{
    private readonly IBurndownRepository _repository;

    public GetBurndownQueryHandler(
        IBurndownRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetBurndownResponse>> Handle(
        GetBurndownQuery request,
        CancellationToken cancellationToken)
    {
        var report = await _repository.GetAsync(
            request.SprintId,
            cancellationToken);

        if (report is null)
        {
            return Result.Failure<GetBurndownResponse>(
                SprintErrors.NotFound);
        }

        return Result.Success(report);
    }
}
