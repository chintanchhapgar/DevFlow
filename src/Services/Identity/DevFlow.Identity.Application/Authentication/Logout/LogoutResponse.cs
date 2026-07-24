using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.Logout;

public sealed record LogoutResponse
    : IEmptyApiResponse
{
    public string Message => "Logged out successfully.";
}
