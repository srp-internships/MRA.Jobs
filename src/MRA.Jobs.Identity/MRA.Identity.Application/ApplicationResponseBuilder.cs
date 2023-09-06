﻿using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Response;

namespace MRA.Identity.Application;

internal class ApplicationResponseBuilder<TResponse> : ApplicationResponse<TResponse>
{
    internal ApplicationResponseBuilder<TResponse> SetErrorMessage(string? errorMessage)
    {
        ErrorMessage = errorMessage;
        return this;
    }

    internal ApplicationResponseBuilder<TResponse> SetResponse(TResponse? response)
    {
        Response = response;
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

    internal ApplicationResponse<List<UserRolesResponse>> SetResponse(List<Task<UserRolesResponse>> list)
    {
        throw new NotImplementedException();
    }
}


internal class ApplicationResponseBuilder : ApplicationResponse
{
    internal ApplicationResponseBuilder SetErrorMessage(string? errorMessage)
    {
        ErrorMessage = errorMessage;
        return this;
    }

    internal ApplicationResponseBuilder SetException(Exception? exception)
    {
        Exception = exception;
        return this;
    }

    internal ApplicationResponseBuilder Success(bool success = true)
    {
        IsSuccess = success;
        return this;
    }

    internal static ApplicationResponseBuilder<TResponse> GetInstance<TResponse>()
    {
        return new ApplicationResponseBuilder<TResponse>();
    }

    internal ApplicationResponse Build()
    {
        return this;
    }
}