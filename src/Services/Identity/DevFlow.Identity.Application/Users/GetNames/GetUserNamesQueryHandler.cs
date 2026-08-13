using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.GetNames;

internal sealed class GetUserNamesQueryHandler
    : IRequestHandler<
        GetUserNamesQuery,
        Result<IReadOnlyList<UserNameResponse>>>
{
    private readonly IUserRepository _userRepository;

    public GetUserNamesQueryHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IReadOnlyList<UserNameResponse>>> Handle(
        GetUserNamesQuery request,
        CancellationToken cancellationToken)
    {
        var userIds = request.UserIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToArray();

        if (userIds.Length == 0)
        {
            return Result.Success<
                IReadOnlyList<UserNameResponse>>(
                [],
                "No users requested.");
        }

        var users = await _userRepository.GetByIdsAsync(
            userIds,
            cancellationToken);

        var response = users
            .Select(user => new UserNameResponse(
                user.Id.Value,
                user.FullName))
            .ToList();

        return Result.Success<
            IReadOnlyList<UserNameResponse>>(
            response,
            "User names retrieved successfully.");
    }
}
