using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Members.List;

public sealed class ListProjectMembersEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/members",
            async (
                Guid projectId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new ListProjectMembersQuery(projectId),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Project Members")
            .WithName("GetProjectMembers")
            .WithSummary("Get project members")
            .WithDescription("Returns all members of the specified project.")
            .Produces<IReadOnlyList<ListProjectMembersResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
