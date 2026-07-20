using DevFlow.Identity.Domain.Authentication;

namespace DevFlow.Identity.Application.Common.Abstractions.Persistence;

/// <summary>
/// Repository contract for the User aggregate root.
/// Only one repository per aggregate root.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    void Add(User user);

    void Update(User user);
}
