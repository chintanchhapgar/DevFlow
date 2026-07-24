using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiresOnUtc)
    : IApiMessage
{
    public string Message => "Login successful.";
}
