using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<CurrentUserResponse>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            UserId.From(request.UserId),
            cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        return new CurrentUserResponse(
            UserId: user.Id.Value,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            FullName: user.FullName,
            IsEmailVerified: user.IsEmailVerified,
            CreatedOnUtc: user.CreatedOnUtc);
    }
}
