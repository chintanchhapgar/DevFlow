using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Identity;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.Add;

internal sealed class AddProjectMemberCommandHandler
    : IRequestHandler<
        AddProjectMemberCommand,
        Result<AddProjectMemberResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserLookupService _userLookupService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public AddProjectMemberCommandHandler(
        IProjectRepository projectRepository,
        IUserLookupService userLookupService,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _userLookupService = userLookupService;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<AddProjectMemberResponse>> Handle(
        AddProjectMemberCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
            return Result.Failure<AddProjectMemberResponse>(ProjectErrors.NotFound);

        if (project.OwnerId != _currentUser.UserId)
            return Result.Failure<AddProjectMemberResponse>(ProjectErrors.Forbidden);

        if (!await _userLookupService.ExistsAsync(request.UserId, cancellationToken))
            return Result.Failure<AddProjectMemberResponse>(ProjectErrors.UserNotFound);

        project.AddMember(request.UserId, request.Role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new AddProjectMemberResponse(
                project.Id.Value,
                request.UserId,
                request.Role.ToString()),
            "Project member added successfully.");
    }
}
