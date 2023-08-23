using MRA.Identity.Application.Contract;

namespace MRA.Identity.Application;

internal class ApplicationResponseBuilder<TResponse>:ApplicationResponse<TResponse>
{
    internal ApplicationResponseBuilder<TResponse> SetErrorMessage(string? errorMessage)
    {
        ErrorMessage = errorMessage;
        return this;
    }
    
    internal ApplicationResponseBuilder<TResponse> SetResponse(TResponse? response)
    {
        Response=response;
        return this;
    }

    internal ApplicationResponseBuilder<TResponse> SetException(Exception? exception)
    {
        Exception = exception;
        return this;
    }

    internal ApplicationResponseBuilder<TResponse> Success(bool success = true)
    {
        IsSuccess = success;
        return this;
    }

    internal static ApplicationResponseBuilder<TResponse> GetInstance<TResponse>()
    {
        return new ApplicationResponseBuilder<TResponse>();
    }

    internal ApplicationResponse<TResponse> Build()
    {
        return this;
    }
}