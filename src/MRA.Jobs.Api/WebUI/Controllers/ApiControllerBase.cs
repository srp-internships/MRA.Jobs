using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MRA.Jobs.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
//[TypeFilter(typeof(BadRequestResponseFilter))]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}