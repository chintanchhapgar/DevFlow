using DevFlow.Identity.Application.Common.Abstractions.Requests;
using Microsoft.AspNetCore.Http;

namespace DevFlow.Identity.Infrastructure.Requests;

/// <summary>
/// Provides information about the current HTTP request.
/// </summary>
public sealed class CurrentRequestInfo : ICurrentRequestInfo
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentRequestInfo(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? DeviceName
    {
        get
        {
            // Placeholder for now.
            // We'll derive a friendly device name later.
            return null;
        }
    }

    public string? Browser
    {
        get
        {
            var userAgent = UserAgent;

            if (string.IsNullOrWhiteSpace(userAgent))
            {
                return null;
            }

            if (userAgent.Contains("Edg/", StringComparison.OrdinalIgnoreCase))
            {
                return "Microsoft Edge";
            }

            if (userAgent.Contains("Chrome/", StringComparison.OrdinalIgnoreCase))
            {
                return "Google Chrome";
            }

            if (userAgent.Contains("Firefox/", StringComparison.OrdinalIgnoreCase))
            {
                return "Mozilla Firefox";
            }

            if (userAgent.Contains("Safari/", StringComparison.OrdinalIgnoreCase) &&
                !userAgent.Contains("Chrome/", StringComparison.OrdinalIgnoreCase))
            {
                return "Safari";
            }

            return "Unknown";
        }
    }

    public string? OperatingSystem
    {
        get
        {
            var userAgent = UserAgent;

            if (string.IsNullOrWhiteSpace(userAgent))
            {
                return null;
            }

            if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
            {
                return "Windows";
            }

            if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
            {
                return "Android";
            }

            if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) ||
                userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase))
            {
                return "iOS";
            }

            if (userAgent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase))
            {
                return "macOS";
            }

            if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase))
            {
                return "Linux";
            }

            return "Unknown";
        }
    }

    public string? IpAddress =>
        _httpContextAccessor
            .HttpContext?
            .Connection
            .RemoteIpAddress?
            .ToString();

    public string? UserAgent =>
        _httpContextAccessor
            .HttpContext?
            .Request
            .Headers
            .UserAgent
            .ToString();
}
