using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Features.Applicant.Query.GetAllApplicant;

namespace MRA.Jobs.Web.Controllers;

public class ApplicantController : ApiControllerBase
{
    private readonly ILogger<ApplicantController> _logger;

    public ApplicantController(ILogger<ApplicantController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApplicant()
    {
        var applicants = await Mediator.Send(new GetAllApplicantQuery());
        return Ok(applicants);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetApplicantById(Guid id)
    {
        var applicant =  Mediator.Send(new GetApplicantByIdQuery { Id = id });
        return Ok(applicant);
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

    [HttpPost("{applicantId}/tags/{tagId}")]
    public async Task<ActionResult<bool>> AddTagToApplicant(Guid applicantId, Guid tagId, CancellationToken cancellationToken)
    {
        var request = new AddTagToApplicantCommand { ApplicantId = applicantId, TagId = tagId };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{applicantId}/tags/{tagId}")]
    public async Task<ActionResult<bool>> RemoveTagFromApplicant(Guid applicantId, Guid tagId, CancellationToken cancellationToken)
    {
        var request = new RemoveTagFromApplicantCommand { ApplicantId = applicantId, TagId = tagId };
        return await Mediator.Send(request, cancellationToken);
    }
}