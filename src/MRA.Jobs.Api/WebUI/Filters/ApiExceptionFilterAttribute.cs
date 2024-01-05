using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MRA.Jobs.Application.Common.Exceptions;

namespace MRA.Jobs.Web.Filters;

public class ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger) : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = context switch
        {
            { Exception: ValidationException } => HandleValidationException(context),
            { Exception: NotFoundException } => HandleNotFoundException(context),
            { Exception: UnauthorizedAccessException } => HandleUnauthorizedAccessException(context),
            { Exception: ForbiddenAccessException } => HandleForbiddenAccessException(context),
            { Exception: TaskCanceledException } => HandleTaskCanceledException(context),
            { Exception: ConflictException } => HandleConflictException(context),
            { ModelState: { IsValid: false } } => HandleInvalidModelStateException(context),
            _ => HandleUnknownException(context)
        };

        base.OnException(context);
    }

    private bool HandleConflictException(ExceptionContext context)
    {
        var exception = (ConflictException)context.Exception;

        var details = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            Detail = exception.Message
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status409Conflict };

        return true;
    }

    public bool HandleTaskCanceledException(ExceptionContext context)
    {
        ProblemDetails details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Request was canceled.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status400BadRequest };

        return true;
    }

    public bool HandleUnknownException(ExceptionContext context)
    {
        ProblemDetails details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };
        logger.LogError(context.Exception, nameof(HandleUnknownException));

        return true;
    }

    private bool HandleValidationException(ExceptionContext context)
    {
        ValidationException exception = (ValidationException)context.Exception;

        ValidationProblemDetails details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        return true;
    }

    private bool HandleInvalidModelStateException(ExceptionContext context)
    {
        ValidationProblemDetails details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        return true;
    }

    private bool HandleNotFoundException(ExceptionContext context)
    {
        ProblemDetails details = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
        };

        context.Result = new NotFoundObjectResult(details);

        return true;
    }

    private bool HandleUnauthorizedAccessException(ExceptionContext context)
    {
        ProblemDetails details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status401Unauthorized };

        return true;
    }

    private bool HandleForbiddenAccessException(ExceptionContext context)
    {
        ProblemDetails details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status403Forbidden };

        return true;
    }
}