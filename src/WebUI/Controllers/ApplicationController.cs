using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;

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

    [HttpGet]
    public async Task<ActionResult<List<ApplicationResponse>>> GetAllApplications(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<ApplicationResponse>> GetApplicationById(GetByIdApplicationQuery request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }


    [HttpPost]
    public async Task<ActionResult<long>> CreateApplication(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<long>> UpdateApplication(int Id, UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        request.Id = Id;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult<bool>> DeleteApplication(int Id, CancellationToken cancellationToken)
    {
        var request = new DeleteApplicationCommand { Id = Id };
        return await Mediator.Send(request, cancellationToken);
    }
}
