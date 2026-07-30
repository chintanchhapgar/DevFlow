using DevFlow.Project.Domain.Epics.Errors;
using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.Project.Domain.Epics.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.GetById;

internal sealed class GetEpicByIdQueryHandler
    : IRequestHandler<
        GetEpicByIdQuery,
        Result<GetEpicByIdResponse>>
{
    private readonly IEpicRepository _epicRepository;

    public GetEpicByIdQueryHandler(
        IEpicRepository epicRepository)
    {
        _epicRepository = epicRepository;
    }

    public async Task<Result<GetEpicByIdResponse>> Handle(
        GetEpicByIdQuery request,
        CancellationToken cancellationToken)
    {
        var epic =
            await _epicRepository.GetByIdAsync(
                new EpicId(request.EpicId),
                cancellationToken);

        if (epic is null)
        {
            return Result.Failure<GetEpicByIdResponse>(
                EpicErrors.NotFound);
        }

        return Result.Success(
            new GetEpicByIdResponse(
                epic.Id.Value,
                epic.ProjectId,
                epic.Name,
                epic.Description,
                epic.Color,
                epic.StartDate,
                epic.DueDate,
                epic.CreatedOnUtc,
                epic.UpdatedOnUtc),
            "Epic retrieved successfully.");
    }
}
