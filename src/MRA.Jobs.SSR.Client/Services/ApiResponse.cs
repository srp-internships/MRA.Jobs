using System.Net;
using MRA.Jobs.Client;

namespace MRA.Jobs.SSR.Client.Services;

public record ApiResponse
{
    public bool Success { get; set; }

    public string Error { get; set; }

    public HttpStatusCode? HttpStatusCode { get; set; }

    public static ApiResponse BuildSuccess(HttpStatusCode? httpStatusCode = null)
    {
        return new ApiResponse
        {
            Success = true,
            HttpStatusCode = httpStatusCode
        };
    }

    public static ApiResponse BuildFailed(string error, HttpStatusCode? httpStatusCode = null)
    {
        return new ApiResponse
        {
            Error = error,
            HttpStatusCode = httpStatusCode,
        };
    }

    public static ApiResponse BuildFailed(ErrorResponse error, HttpStatusCode? httpStatusCode = null)
    {
        return new ApiResponse
        {
            Error = string.Join(',', error.Errors.Select(s => s.Value)),
            HttpStatusCode = httpStatusCode,
        };
    }
}
public record ApiResponse<T> : ApiResponse
{
    public T Result { get; set; }
    public static ApiResponse<T> BuildSuccess(T response, HttpStatusCode? httpStatusCode = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Result = response,
            HttpStatusCode = httpStatusCode
        };
    }

    public static new ApiResponse<T> BuildFailed(string error, HttpStatusCode? httpStatusCode = null)
    {
        return new ApiResponse<T>
        {
            Error = error,
            HttpStatusCode = httpStatusCode,
        };
    }

    public static ApiResponse<T> BuildFailed(T response, HttpStatusCode? httpStatusCode = null)
    {
        return new ApiResponse<T>
        {
            HttpStatusCode = httpStatusCode,
            Result = response
        };
    }

    public new static ApiResponse<T> BuildFailed(ErrorResponse error, HttpStatusCode? httpStatusCode = null)
    {
        return new ApiResponse<T>
        {
            Error = string.Join(',', error.Errors.Select(s => s.Value)),
            HttpStatusCode = httpStatusCode,
        };
    }
}
