namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Generates one-time recovery codes for MFA.
/// </summary>
public interface IRecoveryCodeGenerator
{
    IReadOnlyList<string> Generate(int count = 10);
}
