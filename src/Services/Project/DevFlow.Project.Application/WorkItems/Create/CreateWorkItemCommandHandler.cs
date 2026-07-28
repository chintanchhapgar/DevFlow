using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;
using System.Globalization;

namespace DevFlow.Project.Application.WorkItems.Create;

internal sealed class CreateWorkItemCommandHandler
    : IRequestHandler<CreateWorkItemCommand,
        Result<CreateWorkItemResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateWorkItemCommandHandler(
        IProjectRepository projectRepository,
        IWorkItemRepository workItemRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _workItemRepository = workItemRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<CreateWorkItemResponse>> Handle(
        CreateWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
            return Result.Failure<CreateWorkItemResponse>(
                ProjectErrors.NotFound);

        // temporary key generation
        var key = $"{project.Key}-{Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)[..6].ToUpperInvariant()}";

        var workItem = WorkItemAggregate.Create(
            request.ProjectId,
            key,
            request.Title,
            request.Description,
            request.Type,
            request.Priority,
            _currentUser.UserId,
            request.AssigneeId,
            request.DueDate,
            request.EstimateHours);

        await _workItemRepository.AddAsync(
            workItem,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateWorkItemResponse(
                workItem.Id.Value,
                workItem.Key,
                workItem.Title),
            "Work item created successfully.");
    }
}
