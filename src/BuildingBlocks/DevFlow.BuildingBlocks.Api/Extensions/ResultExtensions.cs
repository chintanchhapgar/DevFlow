using DevFlow.BuildingBlocks.Api.ProblemDetails;
using DevFlow.SharedKernel.Results;
using Microsoft.AspNetCore.Http;

namespace DevFlow.BuildingBlocks.Api.Extensions;

/// <summary>
/// Extension methods for mapping Result types to IResult HTTP responses.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Maps a Result to an HTTP 200 OK or Problem Details response.
    /// </summary>
    public static IResult ToOkOrProblem(this Result result)
    {
        return result.IsSuccess
            ? Results.Ok()
            : ProblemDetailsFactory.ToProblemDetails(result.Error);
    }

    /// <summary>
    /// Maps a Result<T> to an HTTP 200 OK with value or Problem Details response.
    /// </summary>
    public static IResult ToOkOrProblem<TValue>(this Result<TValue> result)
    {
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : ProblemDetailsFactory.ToProblemDetails(result.Error);
    }

    /// <summary>
    /// Maps a Result<T> to an HTTP 201 Created or Problem Details response.
    /// </summary>
    public static IResult ToCreatedOrProblem<TValue>(
        this Result<TValue> result,
        string? uri = null)
    {
        if (result.IsFailure)
            return ProblemDetailsFactory.ToProblemDetails(result.Error);

        return uri is not null
            ? Results.Created(uri, result.Value)
            : Results.Created(string.Empty, result.Value);
    }

    /// <summary>
    /// Maps a Result to an HTTP 204 No Content or Problem Details response.
    /// </summary>
    public static IResult ToNoContentOrProblem(this Result result)
    {
        return result.IsSuccess
            ? Results.NoContent()
            : ProblemDetailsFactory.ToProblemDetails(result.Error);
    }
}
