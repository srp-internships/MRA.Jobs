using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Web.Controllers;

public class ApplicantController : ApiControllerBase
{
    private readonly ILogger<ApplicantController> _logger;

    public ApplicantController(ILogger<ApplicantController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicantDetailsDto>> GetApplicantById(GetApplicantByIdQuery request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApplicant([FromQuery] PaggedListQuery<ApplicantListDto> request)
    {
        var applicants = await Mediator.Send(request);
        return Ok(applicants);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewApplicant(
        CreateApplicantCommand request, CancellationToken cancellationToken
        )
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateApplicant([FromRoute] Guid id,
        [FromBody] UpdateApplicantCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteApplicant(Guid id, [FromBody] DeleteApplicantCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }
}