using DevFlow.SharedKernel.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevFlow.BuildingBlocks.Api.ProblemDetails;

/// <summary>
/// Maps domain errors to RFC 7807 Problem Details responses.
/// Ensures consistent error response format across all services.
/// </summary>
public static class ProblemDetailsFactory
{
    public static IResult ToProblemDetails(AppError error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found",
                detail: error.Description,
                extensions: new Dictionary<string, object?>
                {
                    ["errorCode"] = error.Code
                }),

            ErrorType.Validation => Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: error.Description,
                extensions: new Dictionary<string, object?>
                {
                    ["errorCode"] = error.Code
                }),

            ErrorType.Conflict => Results.Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Conflict",
                detail: error.Description,
                extensions: new Dictionary<string, object?>
                {
                    ["errorCode"] = error.Code
                }),

            ErrorType.Unauthorized => Results.Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized",
                detail: error.Description,
                extensions: new Dictionary<string, object?>
                {
                    ["errorCode"] = error.Code
                }),

            ErrorType.Forbidden => Results.Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden",
                detail: error.Description,
                extensions: new Dictionary<string, object?>
                {
                    ["errorCode"] = error.Code
                }),

            _ => Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error",
                detail: error.Description,
                extensions: new Dictionary<string, object?>
                {
                    ["errorCode"] = error.Code
                })
        };
    }

    public static IResult ToValidationProblemDetails(
        IDictionary<string, string[]> errors)
    {
        return Results.ValidationProblem(
            errors: errors,
            statusCode: StatusCodes.Status400BadRequest,
            title: "Validation Failed");
    }
}
