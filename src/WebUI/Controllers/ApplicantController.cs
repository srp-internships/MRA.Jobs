using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Web.Controllers;

public class ApplicantController : ApiControllerBase
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public ApplicantController(ILogger<OidcConfigurationController> logger)
    {
        _logger = logger;
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