using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Disable;

public sealed record DisableTwoFactorResponse
    : IApiMessage
{
    public string Message => "Two-factor authentication is disabled.";
}
