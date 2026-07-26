namespace DevFlow.SharedKernel.Common;

public interface ICurrentUser
{
    Guid UserId { get; }

    string Email { get; }

    string Name { get; }

    string Role { get; }

    Guid SessionId { get; }

    bool IsAuthenticated { get; }
}
