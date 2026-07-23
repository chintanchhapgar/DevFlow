namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Generates email verification tokens.
/// </summary>
public interface IEmailVerificationTokenGenerator
{
    string Generate();
}
