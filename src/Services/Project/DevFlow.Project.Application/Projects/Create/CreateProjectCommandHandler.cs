using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Entities;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Create;

internal sealed class CreateProjectCommandHandler
    : IRequestHandler<CreateProjectCommand, Result<CreateProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<CreateProjectResponse>> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _projectRepository.ExistsByKeyAsync(
            request.Key,
            cancellationToken);

        if (exists)
        {
            return Result.Failure<CreateProjectResponse>(
                ProjectErrors.DuplicateKey);
        }

        var project = ProjectAggregate.Create(
            request.Key,
            request.Name,
            request.Description,
            _currentUser.UserId,
            request.Visibility);

        await _projectRepository.AddAsync(
            project,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateProjectResponse(
                project.Id.Value,
                project.Key,
                project.Name),
            "Project created successfully.");
    }
}
