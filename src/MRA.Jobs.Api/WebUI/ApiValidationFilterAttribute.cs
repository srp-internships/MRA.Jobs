using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MRA.Jobs.Application.Common.Exceptions;

namespace MRA.Jobs.Application.Contracts;
internal class ApiValidationFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException exception)
        {
            context.ExceptionHandled = true;
            context.Result = new BadRequestObjectResult(exception.Errors);
            return;
        }
        else if (context.Exception is NotFoundException)
        {
            context.ExceptionHandled = true;
            context.Result = new NotFoundResult();
            return;
        }
        base.OnException(context);
    }
}
