using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.ForgotPassword;

public sealed record ForgotPasswordResponse
    : IEmptyApiResponse
{
    public string Message =>
        "If the account exists, password reset instructions have been sent.";
}
