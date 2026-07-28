using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Invitations.GetAll;

public sealed class GetProjectInvitationsEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/invitations",
            async (
                Guid projectId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var query = new GetProjectInvitationsQuery(projectId);

                var result = await sender.Send(
                    query,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Project Invitations")
            .WithName("GetProjectInvitations")
            .WithSummary("Get project invitations")
            .WithDescription("Returns all invitations for a project. Only the project owner can view invitations.")
            .Produces<IReadOnlyList<GetProjectInvitationsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
