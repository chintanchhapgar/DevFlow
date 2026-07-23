using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.SharedKernel.Results;
using Microsoft.AspNetCore.Http;

namespace DevFlow.BuildingBlocks.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(
     this Result<T> result,
     HttpContext context,
     string successMessage = "Success.")
    {
        if (result.IsSuccess)
        {
            return Results.Ok(
                new ApiResponse<T>
                {
                    Success = true,
                    Message = successMessage,
                    Data = result.Value,
                    TraceId = context.TraceIdentifier
                });
        }

        var statusCode = result.Error.Type switch
        {
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            _ => 400
        };

        return Results.Json(
            new ApiResponse<object>
            {
                Success = false,
                Message = result.Error.Description,
                TraceId = context.TraceIdentifier,
                Error = new ApiError
                {
                    Code = result.Error.Code,
                    Message = result.Error.Description,
                    Type = result.Error.Type.ToString()
                }
            },
            statusCode: statusCode);
    }
}
