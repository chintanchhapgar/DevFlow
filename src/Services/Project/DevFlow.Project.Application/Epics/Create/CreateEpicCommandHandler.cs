using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Epics.Entities;
using DevFlow.Project.Domain.Epics.Errors;
using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.Create;

internal sealed class CreateEpicCommandHandler
    : IRequestHandler<
        CreateEpicCommand,
        Result<CreateEpicResponse>>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEpicCommandHandler(
        IEpicRepository epicRepository,
        IUnitOfWork unitOfWork)
    {
        _epicRepository = epicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateEpicResponse>> Handle(
        CreateEpicCommand request,
        CancellationToken cancellationToken)
    {
        var existing =
            await _epicRepository.GetByNameAsync(
                request.ProjectId,
                request.Name,
                cancellationToken);

        if (existing is not null)
        {
            return Result.Failure<CreateEpicResponse>(
                EpicErrors.DuplicateName);
        }

        var epic = EpicAggregate.Create(
            request.ProjectId,
            request.Name,
            request.Description,
            request.Color,
            request.StartDate,
            request.DueDate);

        await _epicRepository.AddAsync(
            epic,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateEpicResponse(
                epic.Id.Value,
                epic.ProjectId,
                epic.Name,
                epic.Description,
                epic.Color,
                epic.StartDate,
                epic.DueDate),
            "Epic created successfully.");
    }
}
