using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.VerifyEmail;

public sealed record VerifyEmailResponse(Guid UserId)
    : IEmptyApiResponse
{
    public string Message => "Email verified successfully.";
}
