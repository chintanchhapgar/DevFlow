using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.BuildingBlocks.Api.Responses
{
    public static class ApiResponseFactory
    {
        public static IResult Success<T>(
            HttpContext context,
            T data,
            string message = "Success.")
        {
            return Results.Ok(
                new ApiResponse<T>
                {
                    Success = true,
                    Message = message,
                    Data = data,
                    TraceId = context.TraceIdentifier
                });
        }
    }
}
