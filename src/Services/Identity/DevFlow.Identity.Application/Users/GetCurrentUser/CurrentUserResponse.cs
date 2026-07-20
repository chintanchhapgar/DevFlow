namespace DevFlow.Identity.Application.Users.GetCurrentUser;

public sealed record CurrentUserResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsEmailVerified,
    DateTime CreatedOnUtc);
