using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MRA.Identity.Api.Filters;
public class BadRequestResponseFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            var errorResponse = new BadRequestResponse
            {
                Errors = errors.Select(e => new Error { Title = "Details", Details = e }).ToList()
            };
            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }
}

public class BadRequestResponse
{
    public List<Error> Errors { get; set; }
}

public class Error
{
    public string Title { get; set; }
    public string Details { get; set; }
}

