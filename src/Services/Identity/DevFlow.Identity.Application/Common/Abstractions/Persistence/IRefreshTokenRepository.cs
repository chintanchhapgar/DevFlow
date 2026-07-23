namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

public interface IRefreshTokenGenerator
{
    string Generate();
}
