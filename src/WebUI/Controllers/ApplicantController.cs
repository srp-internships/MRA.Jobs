using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicantController : ApiControllerBase
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public ApplicantController(ILogger<OidcConfigurationController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult<long>> CreateNewApplicant(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }
}