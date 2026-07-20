using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.UpdateProfile;

internal sealed class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IIdentityUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            UserId.From(request.UserId),
            cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        var updateResult = user.UpdateProfile(request.FirstName, request.LastName);

        if (updateResult.IsFailure)
            return updateResult;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
