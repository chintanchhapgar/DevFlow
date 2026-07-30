using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.GetAll;

internal sealed class GetAllEpicsQueryHandler
    : IRequestHandler<
        GetAllEpicsQuery,
        Result<IReadOnlyList<GetAllEpicsResponse>>>
{
    private readonly IEpicRepository _epicRepository;

    public GetAllEpicsQueryHandler(
        IEpicRepository epicRepository)
    {
        _epicRepository = epicRepository;
    }

    public async Task<Result<IReadOnlyList<GetAllEpicsResponse>>> Handle(
        GetAllEpicsQuery request,
        CancellationToken cancellationToken)
    {
        var epics =
            await _epicRepository.GetByProjectAsync(
                request.ProjectId,
                cancellationToken);

        var response = epics
            .Select(x => new GetAllEpicsResponse(
                x.Id.Value,
                x.ProjectId,
                x.Name,
                x.Description,
                x.Color,
                x.StartDate,
                x.DueDate))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyList<GetAllEpicsResponse>>(
            response,
            "Epics retrieved successfully.");
    }
}
