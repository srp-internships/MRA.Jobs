using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.Applicant.Commands;

namespace MRA.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ApplicantController : ControllerBase
{
    private readonly ISender _mediator;

    public ApplicantController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateApplicantCommand request)
    {
        Guid? result = await _mediator.Send(request);
        return result == null ? BadRequest() : Ok(result);
    }
}
