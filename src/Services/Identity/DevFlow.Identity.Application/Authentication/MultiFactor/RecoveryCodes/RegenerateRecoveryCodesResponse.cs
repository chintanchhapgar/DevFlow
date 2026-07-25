using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.RecoveryCodes;

/// <summary>
/// Response returned after regenerating recovery codes.
/// </summary>
public sealed record RegenerateRecoveryCodesResponse(
    IReadOnlyCollection<string> RecoveryCodes)
    : IApiMessage
{
    public string Message =>
        "Recovery codes regenerated successfully.";
}
