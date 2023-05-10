using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Web.Controllers;

public class ApplicantController : ApiControllerBase
{
    private readonly ILogger<OidcConfigurationController> _loger;

    public ApplicantController(ILogger<OidcConfigurationController> logger)
    {
        _loger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewApplicant(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateApplicant([FromRoute] Guid id,
        [FromBody] UpdateApplicantCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(request, cancellationToken);
    }
}