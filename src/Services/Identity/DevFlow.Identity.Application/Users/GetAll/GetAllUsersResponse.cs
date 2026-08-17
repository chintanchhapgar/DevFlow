namespace DevFlow.Identity.Application.Users.GetAll;

public sealed record UserItemResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    string Role);

public sealed record GetAllUsersResponse(
    IReadOnlyList<UserItemResponse> Users,
    int Page,
    int PageSize,
    int TotalCount);
