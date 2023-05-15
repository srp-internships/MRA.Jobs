using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Features.Applicant.Query.GetAllApplicant;

namespace MRA.Jobs.Web.Controllers;

public class ApplicantController : ApiControllerBase
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public ApplicantController(ILogger<OidcConfigurationController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicantDetailsDto>> GetApplicantById(GetApplicantByIdQuery request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet]
    public async Task<ActionResult<ApplicantDetailsDto>> GetAllApplicant(GetAllApplicantQuery request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
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