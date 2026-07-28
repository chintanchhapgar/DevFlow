using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Members.Add;

public sealed class AddProjectMemberEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/projects/{projectId:guid}/members",
            async (
                Guid projectId,
                AddProjectMemberRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new AddProjectMemberCommand(
                    projectId,
                    request.UserId,
                    request.Role);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithName("AddProjectMember")
            .WithSummary("Add project member")
            .WithDescription("Adds a new member to the project. Only the project owner can add members.")
            .Produces<AddProjectMemberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }
}
