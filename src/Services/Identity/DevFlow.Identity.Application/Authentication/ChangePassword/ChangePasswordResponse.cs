using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.ChangePassword;

public sealed record ChangePasswordResponse
    : IEmptyApiResponse
{
    public string Message => "Password changed successfully.";
}
