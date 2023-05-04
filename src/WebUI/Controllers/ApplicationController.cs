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

    [HttpPut("{id}")]
    public async Task<ActionResult<long>> UpdateApplication([FromQuery] int id, UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        request.Id = id;
        return await Mediator.Send<long>(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteApplication([FromQuery] int id, CancellationToken cancellationToken)
    {
        var request = new DeleteApplicationCommand { Id = id };
        return await Mediator.Send<bool>(request, cancellationToken);
    }
}
