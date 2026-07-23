namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

public interface IPasswordResetTokenGenerator
{
    string Generate();
}
