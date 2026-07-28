using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.Project.Domain.Sprints.Entities;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.Create;

internal sealed class CreateSprintCommandHandler
    : IRequestHandler<
        CreateSprintCommand,
        Result<CreateSprintResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSprintCommandHandler(
        IProjectRepository projectRepository,
        ISprintRepository sprintRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateSprintResponse>> Handle(
        CreateSprintCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<CreateSprintResponse>(
                ProjectErrors.NotFound);
        }

        var sprint = SprintAggregate.Create(
            request.ProjectId,
            request.Name,
            request.Goal,
            request.StartDate,
            request.EndDate);

        await _sprintRepository.AddAsync(
            sprint,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateSprintResponse(
                sprint.Id.Value,
                sprint.ProjectId,
                sprint.Name),
            "Sprint created successfully.");
    }
}
