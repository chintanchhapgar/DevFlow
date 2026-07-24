using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.ResetPassword;

public sealed record ResetPasswordResponse
    : IEmptyApiResponse
{
    public string Message => "Password reset successfully.";
}
