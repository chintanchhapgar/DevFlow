using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.Identity.Application.Users.GetAll;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Identity.Application.Users.GetNames
{

    public sealed class GetUserNamesEndpoint : IEndpoint
    {

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(
            "/api/users/names",
            async (
                [AsParameters] GetUserNamesRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetUserNamesQuery(
                    request.Ids);

                var result = await sender.Send(
                    query,
                    cancellationToken);

                return result;
            })
            .RequireAuthorization()
            .WithTags("Users")
            .WithName("GetUserNames");
        }
    }

    public sealed record GetUserNamesRequest(
    Guid[] Ids);
}
