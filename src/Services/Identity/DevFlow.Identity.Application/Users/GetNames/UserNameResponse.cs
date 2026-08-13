namespace DevFlow.Identity.Application.Users.GetNames;

public sealed record UserNameResponse(
    Guid Id,
    string FullName);
