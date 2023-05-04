using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ApiControllerBase
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public ApplicationController(ILogger<OidcConfigurationController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateApplication(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }
}
