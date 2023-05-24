using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Infrastructure.Shared.Pemission.Responces;
using MRA.Jobs.Web.Controllers;

namespace MRA.Jobs.Web.IdentityControllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaggedList<PermissionResponse>>> Get([FromQuery] PaggedListQuery<PermissionResponse> request)
    {
        return Ok(await _mediator.Send(request));
    }
}