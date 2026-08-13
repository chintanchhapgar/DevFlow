using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace DevFlow.Project.Infrastructure.Identity;

internal sealed class ForwardAuthorizationHandler
    : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ForwardAuthorizationHandler(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorization =
            _httpContextAccessor.HttpContext?
                .Request
                .Headers
                .Authorization
                .FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(authorization))
        {
            request.Headers.Authorization =
                AuthenticationHeaderValue.Parse(authorization);
        }

        return base.SendAsync(
            request,
            cancellationToken);
    }
}
