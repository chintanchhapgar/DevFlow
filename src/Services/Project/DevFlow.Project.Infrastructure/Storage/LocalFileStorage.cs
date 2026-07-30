using DevFlow.Project.Application.Common.Abstractions.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevFlow.Project.Infrastructure.Storage;

internal sealed class LocalFileStorage
    : IFileStorage
{
    private readonly IWebHostEnvironment _environment;

    public LocalFileStorage(
        IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken = default)
    {
        var uploadsRoot = Path.Combine(
            _environment.WebRootPath ?? "wwwroot",
            "uploads",
            folder);

        Directory.CreateDirectory(uploadsRoot);

        var storedName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var fullPath =
            Path.Combine(uploadsRoot, storedName);

        await using var stream =
            File.Create(fullPath);

        await file.CopyToAsync(
            stream,
            cancellationToken);

        return Path.Combine(
            "uploads",
            folder,
            storedName)
            .Replace("\\", "/");
    }

    public Task<Stream> OpenReadAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        Stream stream =
            File.OpenRead(
                Path.Combine(
                    _environment.WebRootPath ?? "wwwroot",
                    path));

        return Task.FromResult(stream);
    }

    public Task DeleteAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        var fullPath =
            Path.Combine(
                _environment.WebRootPath ?? "wwwroot",
                path);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(
    string path,
    CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            File.Exists(
                Path.Combine(
                    _environment.WebRootPath ?? "wwwroot",
                    path)));
    }
}
