using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.SharedKernel.Results;
using Microsoft.AspNetCore.Http;

namespace DevFlow.BuildingBlocks.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(
        this Result<T> result,
        HttpContext context,
        string? successMessage = null)
    {
        if (result.IsSuccess)
        {
            string message = successMessage ?? "";

            if (string.IsNullOrWhiteSpace(message) &&
                result.Value is IApiMessage apiMessage)
            {
                message = apiMessage.Message;
            }

            message ??= "Success.";

            object? data = result.Value;

            if (result.Value is IEmptyApiResponse)
            {
                data = null;
            }

            return Results.Ok(
                new ApiResponse<object?>
                {
                    Success = true,
                    Message = message,
                    Data = data,
                    Error = null,
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                });
        }

        var statusCode = result.Error.Type switch
        {
            DevFlow.SharedKernel.Results.ErrorType.Validation => StatusCodes.Status400BadRequest,
            DevFlow.SharedKernel.Results.ErrorType.NotFound => StatusCodes.Status404NotFound,
            DevFlow.SharedKernel.Results.ErrorType.Conflict => StatusCodes.Status409Conflict,
            DevFlow.SharedKernel.Results.ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            DevFlow.SharedKernel.Results.ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };

        return Results.Json(
            new ApiResponse<object?>
            {
                Success = false,
                Message = result.Error.Description,
                Data = null,
                Error = new ApiError
                {
                    Code = result.Error.Code,
                    Type = MapErrorType(result.Error.Type)
                },
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            },
            statusCode: statusCode);
    }

    private static ErrorType MapErrorType(
        DevFlow.SharedKernel.Results.ErrorType errorType)
    {
        return errorType switch
        {
            DevFlow.SharedKernel.Results.ErrorType.Validation
                => ErrorType.Validation,

            DevFlow.SharedKernel.Results.ErrorType.NotFound
                => ErrorType.NotFound,

            DevFlow.SharedKernel.Results.ErrorType.Conflict
                => ErrorType.Conflict,

            DevFlow.SharedKernel.Results.ErrorType.Unauthorized
                => ErrorType.Unauthorized,

            DevFlow.SharedKernel.Results.ErrorType.Forbidden
                => ErrorType.Forbidden,

            DevFlow.SharedKernel.Results.ErrorType.Failure
                => ErrorType.Failure,

            _ => ErrorType.Unexpected
        };
    }
}
