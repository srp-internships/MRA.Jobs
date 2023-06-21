using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Web.Controllers;
[Authorize]
public class ApplicantController : ApiControllerBase
{
    private readonly ILogger<ApplicantController> _logger;

    public ApplicantController(ILogger<ApplicantController> logger)
    {
        _logger = logger;
    }

  
    [HttpGet]
    public async Task<IActionResult> GetAllApplicant([FromQuery] PaggedListQuery<ApplicantListDto> query)
    {
        var applicants = await Mediator.Send(query);
        return Ok(applicants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetApplicantById([FromRoute] Guid id)
    {
        var applicant = await Mediator.Send(new GetApplicantByIdQuery { Id = id });
        return Ok(applicant);
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewApplicant(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateApplicant([FromRoute] Guid id, [FromBody] UpdateApplicantCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteApplicant([FromRoute] Guid id, [FromBody] DeleteApplicantCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] Guid id, [FromBody] AddTagsToApplicantCommand request, CancellationToken cancellationToken)
    {
        request.ApplicantId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] Guid id, [FromBody] RemoveTagsFromApplicantCommand request, CancellationToken cancellationToken)
    {
        request.ApplicantId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}