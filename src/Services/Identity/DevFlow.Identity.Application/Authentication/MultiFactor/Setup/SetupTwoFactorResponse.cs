namespace DevFlow.Identity.Application.Authentication.MultiFactor.Setup;

public sealed record SetupTwoFactorResponse(
    string ManualEntryKey,
    string QrCodeUri,
    string QrCodeImage);
