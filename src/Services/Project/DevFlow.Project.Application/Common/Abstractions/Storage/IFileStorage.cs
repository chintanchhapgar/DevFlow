using Microsoft.AspNetCore.Http;

namespace DevFlow.Project.Application.Common.Abstractions.Storage;

public interface IFileStorage
{
    Task<string> SaveAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken = default);

    Task<Stream> OpenReadAsync(
        string path,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string path,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string path,
        CancellationToken cancellationToken = default);
}
